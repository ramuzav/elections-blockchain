using System;
using System.Text;

namespace Blockchain
{
    public class StringUtil
    {
        public static string ApplySha256(string input)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                var hex = String.Format("{0:X}", 0xff & theByte);
                if(hex.Length == 1) hash.Append('0');
                hash.Append(hex);
            }
            return hash.ToString();
        }	
    }
}
