using Common;
using System;
using System.Web.Mvc;

namespace CustomFilters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            // //调用原始设置
            // base.OnException(filterContext);
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];

                //接口操作关键参数 -- 由每个接口自行整理提供
                var logParams = filterContext.Controller.ViewData[CustomParam.LogParams].ToString();
                //接口错误代码 -- 代码返回异常统一定义为10
                var retnCode = -1;
                //接口错误消息 -- 捕捉页面异常
                string msgTemplate = filterContext.Exception.Message;

                try
                {
                    LogHelper.WriteLog("fatal", Result.GetIP() + " access " + controllerName + "/" + actionName + " error: " + msgTemplate + "\r\n"
                        + "operParam:" + logParams);
                }
                catch (Exception ee)
                {
                    msgTemplate = msgTemplate + "\r\n" + "写入日志发生异常,异常信息:" + ee.Message;
                }
                finally
                {
                    //避免因日志写入失败而导致结果无法返回
                    filterContext.ExceptionHandled = true;

                    filterContext.Result = new JsonResult
                    {
                        Data = new { code = retnCode, msg = msgTemplate },
                        ContentType = "text/html",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
        }
    }
}
