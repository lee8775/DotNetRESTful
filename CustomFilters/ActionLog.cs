using System;
using System.Web.Mvc;
using Common;

namespace CustomFilters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ActionLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器方法执行之后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //过滤掉添加了[NoActionLog]属性的Action
            object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoActionLog), false);
            object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoActionLog), false);
            if (controllerFilter.Length == 1 || actionFilter.Length == 1)
            {

                filterContext.ExceptionHandled = true;
                return;
            }
            //如果发生异常,直接返回,交给LogException去处理
            if (filterContext.Exception != null)
            {
                return;
            }
            base.OnActionExecuted(filterContext);
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            //接口操作关键参数 -- 由每个接口自行整理提供
            var logParams = filterContext.Controller.ViewData[CustomParam.LogParams].ToString();
            //接口返回代码 -- 由每个接口提供(0表示成功, 其他表示失败)
            var retnCode = filterContext.Controller.ViewData[CustomParam.retnCode].ToString();

            //接口调用成功,则记录用户行为日志user_action_log
            if (retnCode == "0")
            {
                try
                {
                    LogHelper.WriteLog("info", Result.GetIP() + " access " + controllerName + "/" + actionName + "  OK"
                        + "\r\n" + "operParam:" + logParams);
                }
                catch
                {
                    //避免因日志写入失败而导致结果无法返回
                    filterContext.ExceptionHandled = true;
                }
            }
            else //调用失败,则记录用户失败日志user_failure_log
            {
                //接口错误消息 -- 由每个接口提供
                var retnMsg = filterContext.Controller.ViewData[CustomParam.retnMsg].ToString();
                try
                {
                    LogHelper.WriteLog("error", Result.GetIP() + " access " + controllerName + "/" + actionName + " error: " + retnMsg + "\r\n"
                        + "operParam:" + logParams);
                }
                catch
                {
                    //避免因日志写入失败而导致结果无法返回
                    filterContext.ExceptionHandled = true;
                }
            }
        }

        /// <summary>
        /// 控制器方法执行之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //过滤掉添加了[NoActionLog]属性的Action
            object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoActionLog), false);
            object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoActionLog), false);
            if (controllerFilter.Length == 1 || actionFilter.Length == 1)
            {
                return;
            }

            base.OnActionExecuting(filterContext);
            //接口添加默认关键参数为所有GET & POST 的参数
            string query = filterContext.HttpContext.Request.QueryString.ToString();
            string form = filterContext.HttpContext.Request.Form.ToString();
            filterContext.Controller.ViewData[CustomParam.LogParams] = query + "&" + form;
            //默认接口返回代码为0(成功)
            filterContext.Controller.ViewData[CustomParam.retnCode] = 0;
            //默认接口错误消息为""
            filterContext.Controller.ViewData[CustomParam.retnMsg] = "";
        }

        /// <summary>
        /// 视图加载完成后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 视图加载之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
