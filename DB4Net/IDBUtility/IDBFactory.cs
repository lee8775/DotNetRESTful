using System.Data;


namespace DB4Net.IDBUtility
{
    internal interface IDBFactory
    {/// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来获取命令参数中的参数符号oracle为":",sqlserver为"@"
        /// </summary>
        /// <returns></returns>
        string CreateDbParmCharacter();

        string GetConnectionString();

        string FormatColumnName(string ColumnName);

        /// <summary>
        /// 根据配置文件中所配置的数据库类型和传入的
        /// 数据库链接字符串来创建相应数据库连接对象
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateDbConnection();

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库命令对象
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateDbCommand();

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库适配器对象
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter CreateDataAdapter();

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的命令对象来创建相应数据库适配器对象
        /// </summary>
        /// <param name="cmd">数据库命令对象</param>
        /// <returns></returns>
        IDbDataAdapter CreateDataAdapter(IDbCommand cmd);

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        IDbDataParameter CreateDbParameter();

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        IDbDataParameter CreateDbParameter(string paramName, object value);

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <returns></returns>
        IDbDataParameter CreateDbParameter(string paramName, object value, DbType dbType);

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的参数来创建相应数据库的参数数组对象
        /// </summary>
        /// <param name="size">参数数组长度</param>
        /// <returns></returns>
        IDbDataParameter[] CreateDbParameters(int size);

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的事物对象
        /// </summary>
        /// <returns></returns>
        IDbTransaction CreateDbTransaction();
    }
}
