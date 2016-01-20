using System;

namespace CustomFilters
{
    class NoFilter
    {
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoActionLog : Attribute
    {
        public NoActionLog()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoHeadAuth : Attribute
    {
        public NoHeadAuth()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoToken : Attribute
    {
        public NoToken()
        {
        }
    }
}
