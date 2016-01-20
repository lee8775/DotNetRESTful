using DB4Net.IDBUtility;
using System.Data;
using System.Data.SqlClient;

namespace DB4Net.DBUtility
{
    internal class SQLServerFactory : IDBFactory
    { 
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来获取命令参数中的参数符号oracle为":",sqlserver为"@"
        /// </summary>
        /// <returns></returns>
        public string CreateDbParmCharacter()
        {
            return "@";
        }

        public string FormatColumnName(string ColumnName)
        {
            return "[" + ColumnName + "]";
        }

        public string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
        }

        private IDbConnection _conn { get; set; }
        /// <summary>
        /// 根据配置文件中所配置的数据库类型和传入的
        /// 数据库链接字符串来创建相应数据库连接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            if (_conn == null)
            {
                _conn = new SqlConnection(GetConnectionString());
            }
            return _conn;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库命令对象
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateDbCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库适配器对象
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的命令对象来创建相应数据库适配器对象
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            return new SqlDataAdapter((SqlCommand)cmd);
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value)
        {
            IDbDataParameter param = CreateDbParameter();
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value, DbType dbType)
        {
            IDbDataParameter param = CreateDbParameter();
            param.DbType = dbType;
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }


        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的参数来创建相应数据库的参数数组对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter[] CreateDbParameters(int size)
        {
            int i = 0;
            IDbDataParameter[] param = null;

            param = new SqlParameter[size];
            while (i < size) { param[i] = new SqlParameter(); i++; }
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的事物对象
        /// </summary>
        /// <returns></returns>
        public IDbTransaction CreateDbTransaction()
        {
            IDbConnection conn = CreateDbConnection();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn.BeginTransaction();
        }
    }
}
