using System.Web.Mvc;

namespace RESTfulDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        //[CustomFilters.TokenAuthenticator(Order=2)]
        [CustomFilters.NoHeadAuth]

        [CustomFilters.NoToken]
        [CustomFilters.NoActionLog]
        public ActionResult Index()
        {
            string hello = "welcome !";
            return Json(hello, "text/html", JsonRequestBehavior.AllowGet);
        }


    }
}
