using System;

namespace CustomFilters
{
    public static class CustomParam
    {
        public static string LogParams { get { return "LogParams"; } }
        public static string retnCode { get { return "retnCode"; } }
        public static string retnMsg { get { return "retnMsg"; } }
        public static string Authenticator { get { return "Authenticator"; } }
        public static string timeStamp { get { return "timeStamp"; } }
        public static string token { get { return "token"; } }
        public static string openId { get { return "openId"; } }

        public static string jsonData { get { return "jsonData"; } }

        public enum ErrCode
        {
            成功 = 0,
            非法请求 = 2,
            Token失效 = 9
        }

        public static string ErrMsg(int _errCode) { return Enum.GetName(typeof(ErrCode), _errCode); }
    }
}
