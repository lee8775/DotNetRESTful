using System.Collections;
using System.Collections.Generic;

namespace BaseModel
{
    public abstract class BaseEntityObject : DictionaryBase
    {
        /// <summary>  
        /// 实体对象对应数据库中的表名  
        /// </summary>  
        private string _tablename = string.Empty;
        /// <summary>  
        /// 受保护字典:实体类中对应数据库表中主键的属性和属性类型  
        /// </summary>  
        protected Dictionary<string, string> _primarydictionary = new Dictionary<string, string>();
        /// <summary>  
        /// 用于实体子类设置当前子类对应数据库中的表名  
        /// </summary>  
        protected string SetTableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }
        /// <summary>  
        /// 客户代码获取当前实例对应ORM中的表名  
        /// </summary>  
        public string TableName
        {
            get { return _tablename; }
        }
        /// <summary>  
        /// 用于实体子类设置当前实例的属性值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        protected object this[string key]
        {
            get { return this.Dictionary[key]; }
            set { this.Dictionary[key] = value; }
        }
        /// <summary>  
        /// 设置实例的属性值  
        /// </summary>  
        /// <param name="key">属性名称</param>  
        /// <param name="value">属性值</param>  
        public void SetEntityVlues(string key, object value)
        {
            if (this.Dictionary.Contains(key))
            {
                this.Dictionary[key] = value;
            }
        }
        /// <summary>  
        /// 获取实例的属性键值队  
        /// </summary>  
        /// <returns></returns>  
        public IDictionary GetEntityValue
        {
            get { return (IDictionary)this.Dictionary; }
        }
        /// <summary>  
        /// 获取实例的主键信息  
        /// </summary>  
        public IDictionary EntityPrimary
        {
            get { return (IDictionary)_primarydictionary; }
        }
        protected abstract void AddPrimaryToDictionary();
    }
}
