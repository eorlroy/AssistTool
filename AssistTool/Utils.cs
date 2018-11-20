using System.Security.Cryptography;
using System.Text;

namespace AssistTool
{
    class Utils
    {
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);

            b = new MD5CryptoServiceProvider().ComputeHash(b);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
