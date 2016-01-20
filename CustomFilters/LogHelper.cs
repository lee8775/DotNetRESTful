using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace CustomFilters
{
    public class LogHelper
    {
        public static void WriteLog(string type, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            switch (type)
            {
                case "error": log.Error(msg); break; //记录错误日志  
                case "fatal": log.Fatal(msg); break; //记录严重错误  
                case "info": log.Info(msg); break; //记录一般信息  
                case "debug": log.Debug(msg); break; //记录调试信息  
                case "warn": log.Warn(msg); break; //记录警告信息  
                default: log.Info(msg); break;
            }

        }
    }
}
