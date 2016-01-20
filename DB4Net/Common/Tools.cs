using BaseModel;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DB4Net.Common
{
    public class Tools
    {
//        public string CreateMYSQLModel(string DBName, string NameSpaceName, string TableName, string outputFilePath)
//        {
//            if (string.IsNullOrEmpty(DBName) || string.IsNullOrEmpty(NameSpaceName) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(outputFilePath))
//                return "";
//            Session.Session mysql = Session.SessionFactory.GetMySQL();

//            List<TableModel> tbs = mysql.FindAll<TableModel>(@"SELECT COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,COLUMN_COMMENT
// FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='" + DBName + "' AND TABLE_NAME='" + TableName + "' ;");

//            List<KEYModel> keys = mysql.FindAll<KEYModel>(@"SELECT b.COLUMN_NAME,b.DATA_TYPE FROM information_schema.KEY_COLUMN_USAGE a
// inner join information_schema.COLUMNS b on a.TABLE_SCHEMA=b.TABLE_SCHEMA and a.COLUMN_NAME=b.COLUMN_NAME and a.TABLE_NAME=b.TABLE_NAME
// WHERE a.TABLE_SCHEMA='" + DBName + "'  AND a.TABLE_NAME='" + TableName + "' and a.CONSTRAINT_NAME='PRIMARY'; ");

//            return OutPutStr(tbs, keys, NameSpaceName, TableName, outputFilePath);
//        }

        public string TestOracleConn()
        {

            //Session.Session oracle = Session.SessionFactory.GetOracle();

            //Session.Session sql = Session.SessionFactory.GetSQLServer();


            System.Data.DataTable sds = new System.Data.DataTable();
            //sds = oracle.FindAll("select * from INFORMATION ").Tables[0];

            //List<CONFERENCE> tbs = sql.FindAll<CONFERENCE>(@"SELECT * FROM Conference");
            //List<INFORMATION_Oracle> tbs2 = oracle.FindAll<INFORMATION_Oracle>(@"SELECT * FROM INFORMATION");


            int isOk = 0;
            //for (int i = 0; i < tbs.Count; i++)
            //{
                //ParamMap param = oracle.newMap();
                //param.Add("CONFERENCE_ID", tbs[i].ConferenceId);
                //param.Add("CONTENTS", tbs[i].Contents);
                //string sqlStr = "update CONFERENCE set CONTENTS=:CONTENTS  where CONFERENCE_ID=:CONFERENCE_ID ";
                // > 0;

                //CONFERENCE_Oracle o_info = oracle.FindByID<CONFERENCE_Oracle>(tbs[i].ConferenceId.ToString());
                //o_info.CONTENTS = tbs[i].Contents;
                //isOk += oracle.Update<CONFERENCE_Oracle>(o_info);

                //    oracle.ExcuteSQL(sqlStr, param);
                //o_info.INFO_ID = "2015060916531897637858e973c8bb9";
           // }
            return isOk.ToString();
        }

//        public string CreateOracleModel(string DBName, string NameSpaceName, string TableName, string outputFilePath)
//        {
//            if (string.IsNullOrEmpty(DBName) || string.IsNullOrEmpty(NameSpaceName) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(outputFilePath))
//                return "";
//            Session.Session oracle = Session.SessionFactory.GetOracle();

//            List<TableModel> tbs = oracle.FindAll<TableModel>(@"select COLUMN_NAME,case NULLABLE when 'Y' then 'YES' else 'NO' END as IS_NULLABLE
//,DATA_TYPE,DATA_LENGTH as CHARACTER_MAXIMUM_LENGTH,'' as COLUMN_COMMENT from user_tab_columns where table_name=upper('" + TableName + "')");

//            List<KEYModel> keys = oracle.FindAll<KEYModel>(@"select CU.COLUMN_NAME,UT.DATA_TYPE from user_cons_columns cu, user_constraints au ,user_tab_columns ut
//where cu.constraint_name = au.constraint_name and  au.constraint_type = 'P' and cu.TABLE_NAME=ut.TABLE_NAME and CU.COLUMN_NAME=UT.COLUMN_NAME
//AND cu.table_name = '" + TableName + "' ");

//            return OutPutStr(tbs, keys, NameSpaceName, TableName, outputFilePath);
//        }

        public string CreateSQLServerModel(string DBName, string NameSpaceName, string TableName, string outputFilePath)
        {
            if (string.IsNullOrEmpty(DBName) || string.IsNullOrEmpty(NameSpaceName) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(outputFilePath))
                return "";
            Session.Session sqlserver = Session.SessionFactory.GetSQLServer();

            List<TableModel> tbs = sqlserver.FindAll<TableModel>(@" use " + DBName + @" 
declare @TableName varchar(100)
set  @TableName='" + TableName + @"'
SELECT
COLUMN_NAME = a.name,
IS_NULLABLE = case when a.isnullable=1 then 'YES' else 'NO' end ,
DATA_TYPE = b.name,
CHARACTER_MAXIMUM_LENGTH = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
COLUMN_COMMENT=''
FROM syscolumns a left join systypes b on a.xusertype=b.xusertype
            inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
where d.name = @TableName order by a.id,a.colorder ");

            List<KEYModel> keys = sqlserver.FindAll<KEYModel>(@" use " + DBName + @" 
declare @TableName varchar(100)
set  @TableName='" + TableName + @"'
select b.name COLUMN_NAME,d.name DATA_TYPE from
sysobjects a inner join syscolumns b on a.id=b.id
left join systypes d on b.xusertype=d.xusertype
inner join sysobjects c on c.xtype='PK' and c.parent_obj=b.id
where a.xtype='U' and a.name = @TableName and c.name in  (
                     SELECT name FROM sysindexes WHERE indid in ( 
                     SELECT indid FROM sysindexkeys WHERE id = b.id AND colid=b.colid))
 ");

            return OutPutStr(tbs, keys, NameSpaceName, TableName, outputFilePath);
        }

        private string OutPutStr(List<TableModel> tbs, List<KEYModel> keys, string NameSpaceName, string TableName, string outputFilePath)
        {
            //            if (string.IsNullOrEmpty(DBName) || string.IsNullOrEmpty(NameSpaceName) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(outputFilePath))
            //                return "";
            //            Session.Session mysql = Session.SessionFactory.GetMySQL();

            //            List<TableModel> tbs = mysql.Find<TableModel>(@"SELECT COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,COLUMN_COMMENT
            // FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='" + DBName + "' AND TABLE_NAME='" + TableName + "' ;");

            //            List<KEYModel> keys = mysql.Find<KEYModel>(@"SELECT b.COLUMN_NAME,b.DATA_TYPE FROM information_schema.KEY_COLUMN_USAGE a
            // inner join information_schema.COLUMNS b on a.TABLE_SCHEMA=b.TABLE_SCHEMA and a.COLUMN_NAME=b.COLUMN_NAME and a.TABLE_NAME=b.TABLE_NAME
            // WHERE a.TABLE_SCHEMA='" + DBName + "'  AND a.TABLE_NAME='" + TableName + "' and a.CONSTRAINT_NAME='PRIMARY'; ");

            StringBuilder modelStr = new StringBuilder();
            modelStr.Append(@"using System;
using System.ComponentModel.DataAnnotations;

namespace " + NameSpaceName + @"
{
    public class ValuesInit_" + TableName + @" : BaseEntityObject
    {
        public ValuesInit_" + TableName + @"()
        {
            this.SetTableName = " + "\"" + TableName + "\"" + @";
");

            for (int i = 0; i < tbs.Count; i++)
            {
                modelStr.Append(@"
            this.Dictionary.Add(" + "\"" + tbs[i].COLUMN_NAME + "\"" + @", null);
");
            }

            modelStr.Append(@"
            AddPrimaryToDictionary();
        }

        protected override void AddPrimaryToDictionary()
        {");
            for (int i = 0; i < keys.Count; i++)
            {
                modelStr.Append(@"
            _primarydictionary.Add(" + "\"" + keys[i].COLUMN_NAME + "\"" + ", \"" + GetModelDataType(keys[i].DATA_TYPE, "NO") + "\"" + @");");
            }
            modelStr.Append(@"
        }
    }

    public class " + TableName + @" : ValuesInit_" + TableName + @"
    {");
            for (int i = 0; i < tbs.Count; i++)
            {
                string displayName = tbs[i].COLUMN_COMMENT;
                string colName = tbs[i].COLUMN_NAME;
                string typeName = tbs[i].DATA_TYPE;
                string allowNULL = tbs[i].IS_NULLABLE.ToUpper();
                if (string.IsNullOrEmpty(displayName)) { displayName = colName; }

                modelStr.Append(@"
        [ScaffoldColumn(true)]
        [Display(Name = " + "\"" + displayName + "\"" + @", Order = " + (i + 1).ToString() + @")]");
                if (allowNULL == "NO")
                {
                    modelStr.Append(@"
        [Required(ErrorMessage = " + "\"" + "不能为空" + "\"" + @")]");
                }
                ulong? datalen = tbs[i].CHARACTER_MAXIMUM_LENGTH;
                if (datalen != null && datalen > 0)
                {
                    modelStr.Append(@"
        [StringLength(" + datalen.ToString() + @", ErrorMessage = " + "\"" + "长度不可超过" + datalen.ToString() + "\"" + @")]");
                }
                modelStr.Append(@"
        public " + GetModelDataType(typeName, allowNULL) + " " + colName + @"
        {
            get { return this[" + "\"" + colName + "\"" + @"] as " + GetModelDataType(typeName, allowNULL) + @"; }
            set { this[" + "\"" + colName + "\"" + @"] = value; }
        }
");
            }
            modelStr.Append(@"
    }
}");
            //创建文件
            FileStream myFs = new FileStream(outputFilePath + TableName + ".cs", FileMode.Create);
            StreamWriter mySw = new StreamWriter(myFs);
            mySw.Write(modelStr.ToString());
            mySw.Close();
            myFs.Close();

            return modelStr.ToString();
        }

        private string GetModelDataType(string dbDataType, string AllowNULL)
        {
            string typeAdd = "?";//string typeAdd = (AllowNULL.ToUpper() == "NO") ? "" : "?"; // as 后面的参数必须允许为NULL, 所以都要加?
            switch (dbDataType.ToLower())
            {
                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                    return "string";

                case "datetime":
                case "smalldatetime":
                case "time":
                case "date":
                    return "DateTime" + typeAdd;

                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    return "decimal" + typeAdd;
                case "float":
                    return "double" + typeAdd;
                case "real":
                    return "float" + typeAdd;

                case "integer":
                case "int":
                case "smallint":
                case "tinyint":
                    return "int" + typeAdd;

                case "bigint":
                    return "ulong" + typeAdd;
                default: return "string";
            }
        }
    }

    internal class ValuesInit_TableModel : BaseEntityObject
    {
        public ValuesInit_TableModel()
        {
            this.SetTableName = "TableModel";
            this.Dictionary.Add("COLUMN_NAME", null);
            this.Dictionary.Add("IS_NULLABLE", null);
            this.Dictionary.Add("DATA_TYPE", null);
            this.Dictionary.Add("CHARACTER_MAXIMUM_LENGTH", null);
            this.Dictionary.Add("COLUMN_COMMENT", null);
            AddPrimaryToDictionary();
        }
        protected override void AddPrimaryToDictionary()
        {
            _primarydictionary.Add("COLUMN_NAME", "string");
        }
    }
    internal class TableModel : ValuesInit_TableModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string COLUMN_NAME
        {
            get { return this["COLUMN_NAME"] as string; }
            set { this["COLUMN_NAME"] = value; }
        }
        /// <summary>  
        /// 是否允许为空
        /// </summary>  
        public string IS_NULLABLE
        {
            get { return this["IS_NULLABLE"] as string; }
            set { this["IS_NULLABLE"] = value; }
        }
        /// <summary>  
        /// 字段类型
        /// </summary>  
        public string DATA_TYPE
        {
            get { return this["DATA_TYPE"] as string; }
            set { this["DATA_TYPE"] = value; }
        }
        /// <summary>  
        /// 字段长度
        /// </summary>  
        public ulong? CHARACTER_MAXIMUM_LENGTH
        {
            get { return this["CHARACTER_MAXIMUM_LENGTH"] as ulong?; }
            set { this["CHARACTER_MAXIMUM_LENGTH"] = value; }
        }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string COLUMN_COMMENT
        {
            get { return this["COLUMN_COMMENT"] as string; }
            set { this["COLUMN_COMMENT"] = value; }
        }
    }

    internal class ValuesInit_KEYModel : BaseEntityObject
    {
        public ValuesInit_KEYModel()
        {
            this.SetTableName = "KEYModel";
            this.Dictionary.Add("COLUMN_NAME", null);
            this.Dictionary.Add("DATA_TYPE", null);
            AddPrimaryToDictionary();
        }
        protected override void AddPrimaryToDictionary()
        {
            _primarydictionary.Add("COLUMN_NAME", "string");
        }
    }
    internal class KEYModel : ValuesInit_KEYModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string COLUMN_NAME
        {
            get { return this["COLUMN_NAME"] as string; }
            set { this["COLUMN_NAME"] = value; }
        }
        /// <summary>  
        /// 字段类型
        /// </summary>  
        public string DATA_TYPE
        {
            get { return this["DATA_TYPE"] as string; }
            set { this["DATA_TYPE"] = value; }
        }
    }
}
