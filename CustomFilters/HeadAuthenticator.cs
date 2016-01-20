using Common;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace CustomFilters
{
    public class HeadAuthenticatorAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoHeadAuth), false);
            object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoHeadAuth), false);
            if (controllerFilter.Length == 1 || actionFilter.Length == 1)
            {
                return;
            }
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["checkAuthenticator"] == "F")
                return true;
            NameValueCollection _params = httpContext.Request.Params;
            if (_params[CustomParam.Authenticator] == null || _params[CustomParam.timeStamp] == null)
                return false;
            else
            {
                string Authenticator = _params[CustomParam.Authenticator].ToString();
                string timeStamp = _params[CustomParam.timeStamp].ToString();
                return Authenticator == Result.getAuthenticator(timeStamp);
            }
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    ErrorCode = 2,
                    Msg = CustomParam.ErrMsg(2)
                },
                ContentType = "text/html",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
