using System.Web;
using System.Web.Mvc;

namespace RESTfulDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //添加全局头验证
            filters.Add(new CustomFilters.HeadAuthenticatorAttribute(), 1);

            //添加全局Token验证
            filters.Add(new CustomFilters.TokenAuthenticatorAttribute(), 2);
            //添加全局行为日志
            filters.Add(new CustomFilters.ActionLogAttribute(), 3);
            //添加全局异常处理
            filters.Add(new CustomFilters.ExceptionLogAttribute());
        }
    }
}