using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CustomFilters
{
    public class TokenAuthenticatorAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoToken), false);
            object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoToken), false);
            if (controllerFilter.Length == 1 || actionFilter.Length == 1 || System.Configuration.ConfigurationManager.AppSettings["checkToken"] == "F")
            {
                return;
            }
            Stream stream = HttpContext.Current.Request.InputStream;
            if (stream.Length > 0)
            {
                byte[] byts = new byte[stream.Length];
                stream.Read(byts, 0, byts.Length);

                string req = System.Text.Encoding.Default.GetString(byts);
                //JObject jo = JObject.Parse(req);
                filterContext.RouteData.DataTokens.Add(CustomParam.jsonData, req);
            }
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;
            string token = "";
            string openId = "";
            if (httpContext.Request.RequestContext.RouteData.DataTokens[CustomParam.jsonData] != null)
            {
                JObject jo = JObject.Parse(httpContext.Request.RequestContext.RouteData.DataTokens[CustomParam.jsonData].ToString());

                token = jo[CustomParam.token].ToString();
                openId = jo[CustomParam.openId].ToString();
            }

            NameValueCollection _params = httpContext.Request.Params;
            if (_params[CustomParam.token] != null && _params[CustomParam.openId] != null)
            {
                openId = _params[CustomParam.openId].ToString();
                token = Uri.EscapeUriString(_params[CustomParam.token].ToString());
            }
            result = Common.Result.checkUserToken(openId, token);
            return result;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    ErrorCode = 9,
                    Msg = CustomParam.ErrMsg(9)
                },
                ContentType = "text/html",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
