using System.Threading;

namespace DB4Net.Session
{
    internal class SessionThreadLocal
    {
        //SQLServer
        private static ThreadLocal<Session> m_SessionLocal = new ThreadLocal<Session>();
        public static void SetSQLServer(Session session)
        {
            m_SessionLocal.Value = session;
        }

        public static Session GetSQLServer()
        {
            return m_SessionLocal.Value;
        }

        ////MYSQL
        //private static ThreadLocal<Session> sessionLocal_MYSQL = new ThreadLocal<Session>();
        //public static void SetMySQL(Session session)
        //{
        //    sessionLocal_MYSQL.Value = session;
        //}

        //public static Session GetMySQL()
        //{
        //    return sessionLocal_MYSQL.Value;
        //}

        ////Oracle
        //private static ThreadLocal<Session> sessionLocal_ORACLE = new ThreadLocal<Session>();
        //public static void SetOracle(Session session)
        //{
        //    sessionLocal_ORACLE.Value = session;
        //}

        //public static Session GetOracle()
        //{
        //    return sessionLocal_ORACLE.Value;
        //}

        //Clear
        public static void Clear()
        {
            m_SessionLocal.Value = null;
            //sessionLocal_MYSQL.Value = null;
            //sessionLocal_ORACLE.Value = null;
        }
    }
}
