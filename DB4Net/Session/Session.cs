using BaseModel;
using DB4Net.Common;
using DB4Net.DBUtility;
using DB4Net.IDBUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DB4Net.Session
{
    public class Session
    {
        private IDBFactory db;
        private AdoHelper ado;
        private EntityHelper enty;

        private Session(IDBFactory newdb)
        {
            db = newdb;
            ado = new AdoHelper(newdb);
            enty = new EntityHelper(newdb);
        }

        internal static Session PriviteInstance(IDBFactory newdb)
        {
            Session session = new Session(newdb);

            return session;
        }

        public ParamMap newMap()
        {
            return new ParamMap(db);
        }


        #region 将实体数据保存到数据库
        public int Insert<T>(T entity) where T : BaseEntityObject
        {
            if (entity == null) return 0;
            object val = 0;
            try
            {
                //获取SQL语句
                String strSql = enty.GetInsertSql(entity);
                //获取参数
                IDbDataParameter[] parms = enty.GetParameters(entity);
                //执行Insert命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSql, parms);

                ////Access数据库执行不需要命名参数
                //if (ado.DbType == DatabaseType.ACCESS)
                //{
                //    //如果是Access数据库，另外执行获取自动生成的ID
                //    String autoSql = EntityHelper.GetAutoSql();
                //    val = ado.ExecuteScalar(connection, transaction, CommandType.Text, autoSql);
                //}

                //把自动生成的主键ID赋值给返回的对象
                //if (!tableInfo.NoAutomaticKey)
                //{
                //    //if (ado.DbType == DatabaseType.SQLSERVER || ado.DbType == DatabaseType.MYSQL || ado.DbType == DatabaseType.ACCESS || ado.DbType == DatabaseType.SQLITE)
                //    //{
                //        PropertyInfo propertyInfo = EntityHelper.GetPrimaryKeyPropertyInfo(entity, properties);
                //        ReflectionHelper.SetPropertyValue(entity, propertyInfo, val);
                //    //}
                //}
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 批量保存
        public int Insert<T>(List<T> entityList) where T : BaseEntityObject
        {
            if (entityList == null || entityList.Count == 0) return 0;
            object val = 0;

            try
            {
                //从第0行获取实体类信息
                BaseEntityObject firstEntity = entityList[0];
                //获取SQL语句
                String strSQL = enty.GetInsertSql(firstEntity);
                List<IDbDataParameter[]> listCommandParameters = new List<IDbDataParameter[]>();
                foreach (BaseEntityObject entity in entityList)
                {
                    //获取参数
                    IDbDataParameter[] parms = enty.GetParameters(entity);
                    listCommandParameters.Add(parms);
                }
                //执行Insert命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, listCommandParameters);

            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 将实体数据修改到数据库
        public int Update<T>(T entity) where T : BaseEntityObject
        {
            if (entity == null) return 0;
            object val = 0;

            try
            {
                //获取SQL语句
                String strSQL = enty.GetUpdateSql(entity);
                //获取参数
                IDbDataParameter[] parms = enty.GetParameters(entity);
                //执行Update命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 批量更新
        public int Update<T>(List<T> entityList) where T : BaseEntityObject
        {
            if (entityList == null || entityList.Count == 0) return 0;
            object val = 0;

            try
            {
                //从第0行获取实体类信息
                BaseEntityObject firstEntity = entityList[0];
                //获取SQL语句
                String strSQL = enty.GetUpdateSql(firstEntity);

                List<IDbDataParameter[]> listCommandParameters = new List<IDbDataParameter[]>();
                foreach (BaseEntityObject entity in entityList)
                {
                    IDbDataParameter[] parms = enty.GetParameters(entity);
                    listCommandParameters.Add(parms);
                }
                //执行Update命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, listCommandParameters);

            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 执行SQL语句
        public int ExcuteSQL(string strSQL)
        {
            return ExcuteSQL(strSQL, null);
        }

        public int ExcuteSQL(string strSQL, ParamMap param)
        {
            object val = 0;
            try
            {
                //获取参数
                IDbDataParameter[] parms = (param == null) ? null : param.toDbParameters();
                //执行非查询SQL命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 删除实体对应数据库中的数据
        public int Delete<T>(T entity) where T : BaseEntityObject
        {
            if (entity == null) return 0;
            object val = 0;

            try
            {
                //获取SQL语句
                String strSQL = enty.GetDeleteByIdSql(entity);
                //获取参数
                IDbDataParameter[] parms = enty.GetParameters(entity);
                //执行Delete命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 批量删除
        public int Delete<T>(List<T> entityList) where T : BaseEntityObject
        {
            if (entityList == null || entityList.Count == 0) return 0;
            object val = 0;

            try
            {
                //从第0行获取实体类信息
                BaseEntityObject firstEntity = entityList[0];
                //获取SQL语句
                String strSQL = enty.GetDeleteByIdSql(firstEntity);
                List<IDbDataParameter[]> listCommandParameters = new List<IDbDataParameter[]>();
                foreach (BaseEntityObject entity in entityList)
                {
                    IDbDataParameter[] parms = enty.GetParameters(entity);
                    listCommandParameters.Add(parms);
                }
                //执行Delete命令
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, listCommandParameters);

            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 根据主键id删除实体对应数据库中的数据
        public int Delete<T>(object id) where T : BaseEntityObject, new()
        {
            object val = 0;
            try
            {
                T entity = new T();

                String strSQL = enty.GetDeleteByIdSql(entity);
                string key = "";
                foreach (DictionaryEntry Id in entity.EntityPrimary)
                {
                    key = Id.Key.ToString();
                }
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, key, id);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 批量根据主键id删除数据
        public int Delete<T>(object[] ids) where T : BaseEntityObject, new()
        {
            if (ids == null || ids.Length == 0) return 0;
            object val = 0;

            try
            {
                T entity = new T();

                String strSQL = enty.GetDeleteByIdSql(entity);
                string key = "";
                foreach (DictionaryEntry Id in entity.EntityPrimary)
                {
                    key = Id.Key.ToString();
                }
                val = ado.ExecuteNonQuery(CommandType.Text, strSQL, key, ids);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Convert.ToInt32(val);
        }
        #endregion

        #region 通过自定义SQL语句查询数据
        public System.Data.DataSet FindAll(string strSql)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = ado.dataSet(CommandType.Text, strSql, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public List<T> FindAll<T>(string strSql) where T : BaseEntityObject, new()
        {
            List<T> list = new List<T>();
            IDataReader sdr = null;

            try
            {
                T entity = new T();
                sdr = ado.ExecuteReader(CommandType.Text, strSql, null);
                list = enty.toList<T>(sdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return list;
        }

        #endregion

        #region 通过自定义SQL语句查询数据
        public List<T> FindAll<T>(string strSQL, ParamMap param) where T : BaseEntityObject, new()
        {
            List<T> list = new List<T>();
            IDataReader sdr = null;
            try
            {
                T entity = new T();
                sdr = ado.ExecuteReader(CommandType.Text, strSQL, param.toDbParameters());

                list = enty.toList<T>(sdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return list;
        }
        #endregion

        #region 通过主键ID查询数据
        public T FindByID<T>(object id) where T : BaseEntityObject, new()
        {
            T entity = new T();
            IDataReader sdr = null;
            try
            {
                string key = "";
                foreach (DictionaryEntry Id in entity.EntityPrimary)
                {
                    key = Id.Key.ToString();
                }

                String strSQL = enty.GetFindByIdSql(entity);
                sdr = ado.ExecuteReader(CommandType.Text, strSQL, key, id);

                entity = enty.toEntity<T>(sdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return entity;
        }
        #endregion

        #region 通过自定义SQL语句查询数据
        public List<T> FindByPage<T>(string strSql, int start, int limit, ref int total) where T : BaseEntityObject, new()
        {
            List<T> list = new List<T>();
            try
            {
                DataSet ds = ado.dataSet(CommandType.Text, strSql, null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    total = dt.Rows.Count;
                    int startrow = start;
                    int endrow = start + limit;
                    if (total > 0)
                    {
                        T entity = new T();
                        IDictionary model = entity.GetEntityValue;
                        endrow = (total < endrow) ? total : endrow;
                        for (int i = startrow; i < endrow; i++)
                        {

                            T newEntity = new T();
                            foreach (DictionaryEntry column in model)
                            {
                                string key = column.Key.ToString().Trim();
                                newEntity.SetEntityVlues(key, dt.Rows[i][key]);
                            }
                            list.Add(newEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        #endregion


    }
}
