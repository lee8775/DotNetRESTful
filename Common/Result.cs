using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Result
    {
        /// <summary>
        /// 获取字符串类型的主键
        /// </summary>
        /// <returns></returns>
        public static string GetNewId()
        {
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            //创建不重复的Id
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            id += guid.Substring(0, 10);
            return id;
        }

        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // 服务器， using proxy
                    {
                        //得到真实的客户端地址
                        ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); // Return real client IP.
                    }
                    else//如果没有使用代理服务器或者得不到客户端的ip not using proxy or can't get the Client IP
                    {
                        //得到服务端的地址要判断  System.Web.HttpContext.Current 为空的情况
                        ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
                    }
                }
            }
            catch (Exception ep)
            {
                ip = "没有正常获取IP，" + ep.Message;
            }
            return ip;
        }


        /// <summary>
        /// 时间戳获取方法
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string timeStamp(string time)
        {
            DateTime tVal = String.IsNullOrEmpty(time) ? DateTime.Now : Convert.ToDateTime(time);
            //配合JavaScript的标准时间差,即用1970/1/1 08:00作为起始时间计算与当前时间的时间差
            long timeleft = Convert.ToDateTime("1970/1/1 08:00:00").Ticks;
            return Math.Round((tVal.Ticks - timeleft) / 10000 / 1000.0).ToString();
        }

        /// <summary>
        /// 认证头计算函数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string getAuthenticator(string data)
        {
            string md5Secret = "C%A3kh5$78"; //密钥,需转码加密放在webconfig中
            MD5 md5 = new MD5CryptoServiceProvider();
            string src = data + md5Secret;
            byte[] result = Encoding.Default.GetBytes(src);
            byte[] output = md5.ComputeHash(result);
            string str = Convert.ToBase64String(output);
            return str;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string getUserToken(string openId)
        {
            string randData = Math.Floor(new Random().NextDouble() * 10000).ToString();
            string src = "" + randData + "-" + openId + "-" + timeStamp("");
            return getEncryptDes(src);
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] result = Encoding.Default.GetBytes(src);
            //byte[] output = md5.ComputeHash(result);
            //string str = Convert.ToBase64String(output);
            //return str;
        }

        public static bool checkUserToken(string openId, string token)
        {
            string sourceStr = getDecryptDes(token);
            string[] sourceArray = sourceStr.Split('-');
            if (sourceArray.Length != 3)
                return false;
            if (sourceArray[1] != openId)
                return false;
            int currTime = Convert.ToInt32(timeStamp(""));
            if (Convert.ToInt32(sourceArray[2]) + 1000 <= currTime)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        // static byte[] key = strToToHexByte("25C86AB075A9E4AD");
        // static byte[] iv = strToToHexByte("66A4E20210D79730");


        static byte[] key = Convert.FromBase64String("fdbc4y6hdhKlf4M3mjgGrMC3PbryXrxw");//strToToHexByte("25C86AB075A9E4AD");
        static byte[] iv = Convert.FromBase64String("RfnMfrpec48="); //strToToHexByte("66A4E20210D79730");
        /// <summary>
        /// 验证码或密码加密算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string getEncryptDes(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream memStm = new MemoryStream();
            CryptoStream encStream = new CryptoStream(memStm, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] byIn = Encoding.Default.GetBytes(data);
            encStream.Write(byIn, 0, byIn.Length);
            encStream.FlushFinalBlock();
            encStream.Close();
            return Convert.ToBase64String(memStm.ToArray());
        }

        /// <summary>
        /// 验证码或密码解密算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string getDecryptDes(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;
            byte[] byIn = Convert.FromBase64String(data);
            if (byIn == null || byIn.Length == 0) return string.Empty;
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream memStm = new MemoryStream(byIn);
            CryptoStream encStream = new CryptoStream(memStm, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[byIn.Length];
            encStream.Read(fromEncrypt, 0, fromEncrypt.Length);
            encStream.Close();
            string strRet = Encoding.Default.GetString(fromEncrypt);
            return strRet.TrimEnd('\0');
        }

    }
}
