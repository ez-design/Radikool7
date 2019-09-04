using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Radikool7.Classes
{
    public static class Utility
    {
        public static class Text
        {
            public static DateTime StringToDate(string src)
            {
                src = Regex.Replace(src, "[^0-9]", "");
                if (src.Length < 14)
                {
                    return DateTime.MinValue;
                }

                int year = int.TryParse(src.Substring(0, 4), out year) ? year : 0;
                int month = int.TryParse(src.Substring(4, 2), out month) ? month : 0;
                int day = int.TryParse(src.Substring(6, 2), out day) ? day : 0;
                int hour = int.TryParse(src.Substring(8, 2), out hour) ? hour : 0;
                int min = int.TryParse(src.Substring(10, 2), out min) ? min : 0;
                int sec = int.TryParse(src.Substring(12, 2), out sec) ? sec : 0;

                return year == 0
                    ? DateTime.MinValue
                    : new DateTime(year, month, day, hour, min, sec, new GregorianCalendar());
            }
            
            /// <summary>
            /// 暗号化
            /// </summary>
            /// <param name="src"></param>
            /// <param name="encKey"></param>
            /// <returns></returns>
            public static string Encrypt(string src, string encKey)
            {
                if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(encKey)) return "";
                try
                {
                    using (var aes = new AesManaged())
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.IV = Encoding.UTF8.GetBytes(encKey.Substring(0, 16));
                        aes.Key = Encoding.UTF8.GetBytes(encKey);
                        aes.Padding = PaddingMode.PKCS7;

                        var srcByte = Encoding.UTF8.GetBytes(src);
                        var encrypt = aes.CreateEncryptor().TransformFinalBlock(srcByte, 0, srcByte.Length);
                        return Convert.ToBase64String(encrypt);
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            
            /// <summary>
            /// 復号化
            /// </summary>
            /// <param name="src"></param>
            /// <param name="encKey"></param>
            /// <returns></returns>
            public static string Decrypt(string src, string encKey)
            {
                if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(encKey)) return "";

                try
                {
                    using (var aes = new AesManaged())
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.IV = Encoding.UTF8.GetBytes(encKey.Substring(0, 16));
                        aes.Key = Encoding.UTF8.GetBytes(encKey);
                        aes.Padding = PaddingMode.PKCS7;

                        var srcByte = Convert.FromBase64String(src);
                        var decrypt = aes.CreateDecryptor().TransformFinalBlock(srcByte, 0, srcByte.Length);
                        return Encoding.UTF8.GetString(decrypt);
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }

        }
    }
}