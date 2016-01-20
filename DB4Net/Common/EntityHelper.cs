using BaseModel;
using DB4Net.IDBUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DB4Net.Common
{
    internal class EntityHelper
    {
        private IDBFactory db;
        internal EntityHelper(IDBFactory newdb)
        {
            db = newdb;
        }

        internal IDbDataParameter[] GetParameters(BaseEntityObject entity)
        {
            IDictionary models = entity.GetEntityValue;
            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            foreach (DictionaryEntry columns in models)
            {
                string key = columns.Key.ToString().Trim();
                object value = columns.Value;
                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    IDbDataParameter param = db.CreateDbParameter();
                    param.ParameterName = key;
                    param.Value = value;

                    paramList.Add(param);
                }
            }

            int i = 0;
            IDbDataParameter[] parameters = db.CreateDbParameters(paramList.Count);
            foreach (IDbDataParameter dbParameter in paramList)
            {
                parameters[i] = dbParameter;
                i++;
            }

            return parameters;
        }
        internal string GetInsertSql(BaseEntityObject entity)
        {
            string TableName = entity.TableName;
            IDictionary models = entity.GetEntityValue;


            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            //if (tableInfo.Strategy != GenerationType.INDENTITY)
            //{
            //    if (tableInfo.Strategy == GenerationType.GUID && tableInfo.Id.Value == null)
            //    {
            //        tableInfo.Id.Value = Guid.NewGuid().ToString();
            //    }

            //    if (tableInfo.Id.Value != null)
            //    {
            //        tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);
            //    }
            //}
            //IDictionary models = entity.GetEntityValue;
            foreach (DictionaryEntry columns in models)
            {
                Object value = columns.Value;
                string key = columns.Key.ToString().Trim();
                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    string nKey = db.FormatColumnName(key);
                    sbColumns.Append(nKey).Append(",");
                    sbValues.Append(db.CreateDbParmCharacter()).Append(key).Append(",");
                }
            }

            if (sbColumns.Length > 0 && sbValues.Length > 0)
            {
                sbColumns.Remove(sbColumns.ToString().Length - 1, 1);
                sbValues.Remove(sbValues.ToString().Length - 1, 1);
            }

            string strSql = "INSERT INTO {0}({1}) VALUES({2})";
            strSql = string.Format(strSql, TableName, sbColumns.ToString(), sbValues.ToString());


            //if (!tableInfo.NoAutomaticKey)
            //{
            //    if (AdoHelper.DbType == DatabaseType.SQLSERVER || AdoHelper.DbType == DatabaseType.MYSQL)
            //    {
            //        string autoSql = EntityHelper.GetAutoSql();
            //        strSql = strSql + autoSql;
            //    }
            //}

            return strSql;
        }

        internal string GetUpdateSql(BaseEntityObject entity)
        {
            string TableName = entity.TableName;
            IDictionary Ids = entity.EntityPrimary;
            IDictionary models = entity.GetEntityValue;

            StringBuilder sbBody = new StringBuilder();

            foreach (DictionaryEntry columns in models)
            {
                Object value = columns.Value;
                string key = columns.Key.ToString().Trim();
                if (!string.IsNullOrEmpty(key.Trim()) && value != null)
                {
                    string nKey = db.FormatColumnName(key);
                    sbBody.Append(",").Append(nKey).Append("=").Append(db.CreateDbParmCharacter() + key);
                }
            }
            if (sbBody.Length > 0) sbBody.Remove(0, 1);

            StringBuilder sbWhere = new StringBuilder();

            foreach (DictionaryEntry Id in Ids)
            {
                Object value = Id.Value;
                string key = Id.Key.ToString().Trim();
                if (!string.IsNullOrEmpty(key.Trim()) && value != null)
                {
                    string nKey = db.FormatColumnName(key);
                    sbWhere.Append(" and ").Append(nKey).Append("=").Append(db.CreateDbParmCharacter() + key);
                }
            }

            if (sbWhere.Length > 0) sbWhere.Remove(0, 5);

            string strSql = "update {0} set {1} where {2} ";
            strSql = string.Format(strSql, TableName, sbBody.ToString(), sbWhere);

            return strSql;
        }

        internal string GetDeleteByIdSql(BaseEntityObject entity)
        {
            string TableName = entity.TableName;
            IDictionary Ids = entity.EntityPrimary;

            StringBuilder sbWhere = new StringBuilder();

            foreach (DictionaryEntry Id in Ids)
            {
                Object value = Id.Value;
                string key = Id.Key.ToString().Trim();
                if (!string.IsNullOrEmpty(key.Trim()) && value != null)
                {
                    string nKey = db.FormatColumnName(key);
                    sbWhere.Append(" and ").Append(nKey).Append("=").Append(db.CreateDbParmCharacter() + key);
                }
            }

            if (sbWhere.Length > 0) sbWhere.Remove(0, 5);

            string strSql = "delete from {0} where {1} ";
            strSql = string.Format(strSql, TableName, sbWhere);

            return strSql;
        }


        internal string GetFindByIdSql(BaseEntityObject entity)
        {
            string TableName = entity.TableName;
            IDictionary Ids = entity.EntityPrimary;
            IDictionary models = entity.GetEntityValue;

            StringBuilder sbColumns = new StringBuilder();

            foreach (DictionaryEntry columns in models)
            {
                string nKey = db.FormatColumnName(columns.Key.ToString().Trim());
                sbColumns.Append(",").Append(nKey);
            }
            if (sbColumns.Length > 0) sbColumns.Remove(0, 1);

            StringBuilder sbWhere = new StringBuilder();

            foreach (DictionaryEntry Id in Ids)
            {
                Object value = Id.Value;
                string key = Id.Key.ToString().Trim();
                if (!string.IsNullOrEmpty(key.Trim()) && value != null)
                {
                    string nKey = db.FormatColumnName(key);
                    sbWhere.Append(" and ").Append(nKey).Append("=").Append(db.CreateDbParmCharacter() + key);
                }
            }

            if (sbWhere.Length > 0) sbWhere.Remove(0, 5);

            string strSql = "SELECT {0} FROM {1} WHERE {2} ";
            strSql = string.Format(strSql, sbColumns.ToString(), TableName, sbWhere);

            return strSql;
        }

        internal T toEntity<T>(IDataReader sdr) where T : BaseEntityObject, new()
        {
            T entity = new T();
            IDictionary model = entity.GetEntityValue;
            T newEntity = new T();
            if (sdr.Read())
            {
                foreach (DictionaryEntry column in model)
                {
                    string key = column.Key.ToString().Trim();
                    newEntity.SetEntityVlues(key, sdr[key]);
                }
            }
            return newEntity;
        }
        internal List<T> toList<T>(IDataReader sdr) where T : BaseEntityObject, new()
        {
            List<T> list = new List<T>();

            T entity = new T();
            IDictionary model = entity.GetEntityValue;

            while (sdr.Read())
            {
                T newEntity = new T();
                foreach (DictionaryEntry column in model)
                {
                    string key = column.Key.ToString().Trim();
                    newEntity.SetEntityVlues(key, sdr[key]);
                }
                list.Add(newEntity);
            }
            return list;
        }
    }
}
