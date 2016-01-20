
namespace DB4Net.Session
{
    public class SessionFactory
    {
        public static Session GetSQLServer()
        {
            Session session = SessionThreadLocal.GetSQLServer();
            if (session == null)
            {
                session = Session.PriviteInstance(new DBUtility.SQLServerFactory());
                SessionThreadLocal.SetSQLServer(session);
            }

            return session;
        }

        //public static Session GetMySQL()
        //{
        //    Session session = SessionThreadLocal.GetMySQL();
        //    if (session == null)
        //    {
        //        session = Session.PriviteInstance(new DBUtility.MySQLFactory());
        //        SessionThreadLocal.SetMySQL(session);
        //    }

        //    return session;
        //}

        //public static Session GetOracle()
        //{
        //    Session session = SessionThreadLocal.GetOracle();
        //    if (session == null)
        //    {
        //        session = Session.PriviteInstance(new DBUtility.OracleFactory());
        //        SessionThreadLocal.SetOracle(session);
        //    }

        //    return session;
        //}
    }
}
