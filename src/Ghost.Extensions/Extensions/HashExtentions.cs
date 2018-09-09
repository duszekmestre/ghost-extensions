using System;
using System.Text;

namespace Ghost.Extensions.Extensions
{
    public static class HashExtentions
    {
        public static string MD5(this byte[] byteArray)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
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

        public static string SHA1(this string plainText)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha.ComputeHash(Encoding.Default.GetBytes(plainText));
                return Encoding.Default.GetString(hash);
            }
        }

        public static string SHA1<T>(this string plainText)
            where T : Encoding, new()
        {
            var encoding = new T();
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha.ComputeHash(encoding.GetBytes(plainText));
                return encoding.GetString(hash);
            }
        }

        public static string SHA1Base64(this string plainText)
        {
            var data = Encoding.UTF8.GetBytes(plainText);
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                data = sha1.ComputeHash(data);
            }

            return Convert.ToBase64String(data);
        }
    }
}
