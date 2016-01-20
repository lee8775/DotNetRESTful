using System;
using System.ComponentModel.DataAnnotations;

namespace BaseModel
{
    public class ValuesInit_AppUpdate : BaseEntityObject
    {
        public ValuesInit_AppUpdate()
        {
            this.SetTableName = "AppUpdate";

            this.Dictionary.Add("Id", null);

            this.Dictionary.Add("CompanyId", null);

            this.Dictionary.Add("clienttype", null);

            this.Dictionary.Add("Name", null);

            this.Dictionary.Add("VersionNO", null);

            this.Dictionary.Add("FileURL", null);

            this.Dictionary.Add("PublishPersonId", null);

            this.Dictionary.Add("PublishTime", null);

            this.Dictionary.Add("UpdateTime", null);

            this.Dictionary.Add("UpdatePersonId", null);

            AddPrimaryToDictionary();
        }

        protected override void AddPrimaryToDictionary()
        {
            _primarydictionary.Add("Id", "string");
        }
    }

    public class AppUpdate : ValuesInit_AppUpdate
    {
        [ScaffoldColumn(true)]
        [Display(Name = "Id", Order = 1)]
        [Required(ErrorMessage = "不能为空")]
        public string Id
        {
            get { return this["Id"] as string; }
            set { this["Id"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "CompanyId", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        public string CompanyId
        {
            get { return this["CompanyId"] as string; }
            set { this["CompanyId"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "clienttype", Order = 3)]
        public string clienttype
        {
            get { return this["clienttype"] as string; }
            set { this["clienttype"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "Name", Order = 4)]
        public string Name
        {
            get { return this["Name"] as string; }
            set { this["Name"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "VersionNO", Order = 5)]
        [Required(ErrorMessage = "不能为空")]
        public string VersionNO
        {
            get { return this["VersionNO"] as string; }
            set { this["VersionNO"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "FileURL", Order = 6)]
        [Required(ErrorMessage = "不能为空")]
        public string FileURL
        {
            get { return this["FileURL"] as string; }
            set { this["FileURL"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "PublishPersonId", Order = 7)]
        public string PublishPersonId
        {
            get { return this["PublishPersonId"] as string; }
            set { this["PublishPersonId"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "PublishTime", Order = 8)]
        public DateTime? PublishTime
        {
            get { return this["PublishTime"] as DateTime?; }
            set { this["PublishTime"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "UpdateTime", Order = 9)]
        public DateTime? UpdateTime
        {
            get { return this["UpdateTime"] as DateTime?; }
            set { this["UpdateTime"] = value; }
        }

        [ScaffoldColumn(true)]
        [Display(Name = "UpdatePersonId", Order = 10)]
        public string UpdatePersonId
        {
            get { return this["UpdatePersonId"] as string; }
            set { this["UpdatePersonId"] = value; }
        }

    }
}