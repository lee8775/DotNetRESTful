using System.Linq;
using System.Web.Mvc;
using DAL;
using BaseModel;
using System.IO;
using Newtonsoft.Json.Linq;
using CustomFilters;

namespace RESTfulDemo.Controllers
{
    public class AppUpdateController : Controller
    {
        //
        // GET: /AppUpdate/
        AppUpdateRepository a_Rep = new AppUpdateRepository();

        //[CustomFilters.NoToken]
        public JsonResult GetVersion()
        {
            JObject jo = JObject.Parse(RouteData.DataTokens[CustomParam.jsonData].ToString());

            string id = jo["id"].ToString();

            AppUpdate au = a_Rep.GetById(id);// db.AppUpdate.ToList().Find(s => s.CompanyId == person.CompanyId && s.clienttype == appType);

            JsonResult json = new JsonResult
            {
                Data = new
                {
                    version = au.VersionNO,
                    name = au.Name,
                    softwareUrl = au.FileURL,
                }
            };
            return Json(json.Data, "text/html", JsonRequestBehavior.AllowGet);
        }

    }
}
