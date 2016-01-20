using DB4Net.IDBUtility;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB4Net.DBUtility
{
    internal class AdoHelper
    {
        private IDBFactory db;

        private IDbTransaction m_Transaction = null;

        internal AdoHelper(IDBFactory newdb)
        {
            db = newdb;
        }

        private IDbConnection GetConnection()
        {
            //获取数据库连接，如果开启了事务，从事务中获取
            IDbConnection connection = null;
            if (m_Transaction != null)
            {
                connection = m_Transaction.Connection;
            }
            else
            {
                connection = db.CreateDbConnection();
            }

            return connection;
        }

        internal void BeginTransaction()
        {
            m_Transaction = db.CreateDbTransaction();
        }

        internal void Commit()
        {
            if (m_Transaction != null && m_Transaction.Connection != null)
            {
                if (m_Transaction.Connection.State != ConnectionState.Closed)
                {
                    m_Transaction.Commit();
                    m_Transaction = null;
                }
            }
        }

        internal void Rollback()
        {
            if (m_Transaction != null && m_Transaction.Connection != null)
            {
                if (m_Transaction.Connection.State != ConnectionState.Closed)
                {
                    m_Transaction.Rollback();
                    m_Transaction = null;
                }
            }
        }

        private IDbTransaction GetTransaction()
        {
            return m_Transaction;
        }

        private bool GetWillConnectionState()
        {
            return m_Transaction == null;
        }

        internal int ExecuteNonQuery(CommandType cmdType, string cmdText, string key, object id)
        {
            IDbDataParameter[] commandParameters = db.CreateDbParameters(1);
            IDbDataParameter param = db.CreateDbParameter();
            param.ParameterName = key;
            param.Value = id;
            commandParameters[0] = param;
            return ExecuteNonQuery(cmdType, cmdText, commandParameters);
        }

        internal int ExecuteNonQuery(CommandType cmdType, string cmdText, string key, object[] ids)
        {
            List<IDbDataParameter[]> listCommandParameters = new List<IDbDataParameter[]>();
            foreach (object id in ids)
            {
                IDbDataParameter[] commandParameters = db.CreateDbParameters(1);
                IDbDataParameter param = db.CreateDbParameter();
                param.ParameterName = key;
                param.Value = id;
                commandParameters[0] = param;
                listCommandParameters.Add(commandParameters);
            }
            return ExecuteNonQuery(cmdType, cmdText, listCommandParameters);
        }
        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        internal int ExecuteNonQuery(CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {


            IDbCommand cmd = db.CreateDbCommand();
            int val = 0;
            BeginTransaction();
            IDbConnection connection = GetConnection();
            IDbTransaction transaction = GetTransaction();
            try
            {
                PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
                val = cmd.ExecuteNonQuery();
                Commit();
            }
            catch (Exception ex)
            {
                Rollback();

                throw ex;
            }
            finally
            {

                cmd.Parameters.Clear();
                if (m_Transaction == null)
                {
                    connection.Close();
                }
            }
            return val;
        }

        internal int ExecuteNonQuery(CommandType cmdType, string cmdText, List<IDbDataParameter[]> listCommandParameters)
        {
            int val = 0;
            BeginTransaction();
            IDbConnection connection = GetConnection();
            IDbTransaction transaction = GetTransaction();
            try
            {
                foreach (IDbDataParameter[] commandParameters in listCommandParameters)
                {
                    IDbCommand cmd = db.CreateDbCommand();
                    PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                Commit();
            }
            catch (Exception ex)
            {
                Rollback();

                throw ex;
            }
            finally
            {
                if (m_Transaction == null)
                {
                    connection.Close();
                }
            }
            return val;
        }

        internal IDataReader ExecuteReader(CommandType cmdType, string cmdText, string key, object id)
        {
            IDbDataParameter[] commandParameters = db.CreateDbParameters(1);
            IDbDataParameter param = db.CreateDbParameter();
            param.ParameterName = key;
            param.Value = id;
            commandParameters[0] = param;
            return ExecuteReader(cmdType, cmdText, commandParameters);
        }
        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回SqlDataReader对象</returns>
        internal IDataReader ExecuteReader(CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            bool closeConnection = GetWillConnectionState();
            IDbConnection conn = GetConnection();

            IDbCommand cmd = db.CreateDbCommand();
            //IDbConnection conn = connection;

            //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
            //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
            //将不会执行。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDataReader rdr = closeConnection ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns>数据集DataSet对象</returns>
        internal DataSet dataSet(CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection conn = GetConnection();

            IDbCommand cmd = db.CreateDbCommand();
            //IDbConnection conn = connection;

            DataSet ds = new DataSet();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = db.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="trans">IDbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (IDbDataParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
