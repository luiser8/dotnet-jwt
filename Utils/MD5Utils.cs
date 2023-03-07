using System.Text;

namespace DotnetJWT.MD5Utils
{
    public static class Md5utils
    {
        private static System.Security.Cryptography.MD5 md5String;

        public static string GetMD5(string password)
        {
            md5String = System.Security.Cryptography.MD5.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder stringBuilder = new StringBuilder();
            stream = md5String.ComputeHash(encoding.GetBytes(password));

            for (int i = 0; i < stream.Length; i++) stringBuilder.AppendFormat("{0:x2}", stream[i]);
            return stringBuilder.ToString();
        }
    }
}