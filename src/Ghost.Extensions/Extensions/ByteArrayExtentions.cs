using System;
using System.Security.Cryptography;
using System.Text;

namespace Ghost.Extensions.Extensions
{
    public static class ByteArrayExtentions
    {
        public static string GetMD5(this byte[] byteArray)
        {
            using (var md5 = MD5.Create())
            {
                var hashTmp = md5.ComputeHash(byteArray);
                return Convert.ToBase64String(hashTmp);
            }           
        }

        public static string SHA1(this byte[] byteArray)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha.ComputeHash(byteArray);
                return Encoding.Default.GetString(hash);
            }
        }

        public static string SHA1<T>(this byte[] byteArray)
            where T : Encoding, new()
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha.ComputeHash(byteArray);
                return new T().GetString(hash);
            }
        }
    }
}
