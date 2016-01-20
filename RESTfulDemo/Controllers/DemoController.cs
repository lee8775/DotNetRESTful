using System.Web.Mvc;

namespace RESTfulDemo.Controllers
{
    public class DemoController : Controller
    {

        [HttpGet]
        //[CustomFilters.TokenAuthenticator(Order = 2)]
        public ActionResult TestGet()
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    code = 0,
                    msg = "this is a GET request!"
                }
            };
            return Json(json.Data, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TestPost()
        {
            JsonResult json2 = new JsonResult
            {
                Data = new
                {
                    code = 0,
                    msg = "this is a POST request!"
                }
            };
            return Json(json2.Data, "text/html", JsonRequestBehavior.AllowGet);
        }



        [CustomFilters.NoHeadAuth]
        [CustomFilters.NoToken]
        [CustomFilters.NoActionLog]
        public string AddModel(string tbName)
        {
            string res = new DAL.SysEntity().AddModel(tbName);
            ViewBag.ModelText = res;
            return res;
        }


        [CustomFilters.NoToken]
        public ActionResult GetToken(string openId)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    code = 0,
                    msg = "Get Token Sucess!",
                    token = Common.Result.getUserToken(openId)
                }
            };
            return Json(json.Data, "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}
