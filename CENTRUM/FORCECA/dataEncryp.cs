using System;
using System.IO;
using System.Text;  
using System.Data;
using System.Configuration;
using System.Web;

namespace FORCECA
{
    /// <summary>
    /// 
    /// </summary>
    public static class dataEncryp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string EncryptText(string strText)
        {
            byte[] EncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(strText);
            string returnValue = System.Convert.ToBase64String(EncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DecryptText(string strText)
        {
            byte[] DecodeAsBytes = System.Convert.FromBase64String(strText);
            string returnValue = System.Text.Encoding.UTF8.GetString(DecodeAsBytes);
            return returnValue;
        }        
    }
}