using System.Collections.Generic;
using System.Data;

namespace DB4Net.Common
{
    public class ParamMap
    {
        private IDBUtility.IDBFactory db;

        private List<IDbDataParameter> m_ParamList = new List<IDbDataParameter>();

        internal ParamMap(IDBUtility.IDBFactory newdb) { db = newdb; }

        public void Add(object key, object value)
        {
            IDbDataParameter param = db.CreateDbParameter(key.ToString(), value);
            if (m_ParamList.Contains(param)) m_ParamList.Remove(param);
            m_ParamList.Add(param);
        }

        internal IDbDataParameter[] toDbParameters()
        {
            int i = 0;
            IDbDataParameter[] paramArr = db.CreateDbParameters(m_ParamList.Count);
            foreach (IDbDataParameter dbParameter in m_ParamList)
            {
                paramArr[i] = dbParameter;
                i++;
            }

            return paramArr;
        }
    }
}
