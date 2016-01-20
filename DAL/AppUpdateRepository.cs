using BaseModel;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AppUpdateRepository : SysEntity
    {
        public AppUpdate GetById(string Id)
        {
            return DB.FindByID<AppUpdate>(Id);
        }

        public List<AppUpdate> GetAll()
        {
            return DB.FindAll<AppUpdate>("select  * from AppUpdate ");
        }

        public bool EditVersion(string picId)
        {
            DB4Net.Common.ParamMap param = DB.newMap();
            param.Add("picId", picId);
            string sql = "update AdvertPicture set ScanAccount=isnull(ScanAccount,0)+1  where PicId=@picId ";
            return DB.ExcuteSQL(sql, param) > 0;
        }
    }
}
