using DB4Net.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SysEntity
    {
        protected Session DB = SessionFactory.GetSQLServer();

        //用代码自动生成模型实体类
        public string AddModel(string tbName)
        {
            DB4Net.Common.Tools tool = new DB4Net.Common.Tools();

            string sqlTb = tool.CreateSQLServerModel("test", "BaseModel", tbName, @"D:\");//
            //string oracleTb = tool.CreateOracleModel("zhlh_qg", "BaseModel", tbName, @"D:\");//\gongzhong\rbs\picc\BaseModel
            //return sqlTb + oracleTb;

            //string sqlTb = tool.TestOracleConn();//\gongzhong\rbs\picc\BaseModel

            return sqlTb;
        }
    }
}
