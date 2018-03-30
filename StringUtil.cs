using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public static String GetStringFromKey(string key)
        {
            var encodedBytes = Encoding.Unicode.GetBytes(key);
            return  Convert.ToBase64String(encodedBytes);
        }

        public static byte[] SignData(string privateKey, string input)
        {
            // convert the string into byte array
            byte[] str = ASCIIEncoding.Unicode.GetBytes(input);

            var sha1hash = new SHA1Managed();
            var hashdata = sha1hash.ComputeHash(str);

            // sign the hash data with private key
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringInternal(privateKey);
            //  signature hold the sign data of plaintext , signed by private key
            return rsa.SignData(str, "SHA1");
        }

        public static bool VerifyData(string publicKey, string data, byte[] signature)
        {
            var str = ASCIIEncoding.Unicode.GetBytes(data);

            // compute the hash again, also we can pass it as a parameter
            var sha1hash = new SHA1Managed();
            var hashdata = sha1hash.ComputeHash(str);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringInternal(publicKey);

            return rsa.VerifyHash(hashdata, "SHA1", signature);
        }


        // NOT AN ACTUAL MERKLE ROOT -- NEED TO REVIEW AND IMPLEMENT CORRECT ONE
        public static String GetMerkleRoot(List<Transaction> transactions)
        {
            int count = transactions.Count;
            var previousTreeLayer = new List<string>();

            foreach (var transaction in transactions)
            {
                previousTreeLayer.Add(transaction.TransactionId);
            }

            var treeLayer = previousTreeLayer;

            while (count > 1)
            {
                treeLayer = new List<string>();
                for (int i = 1; i < previousTreeLayer.Count; i++)
                {
                    treeLayer.Add(ApplySha256(previousTreeLayer[i - 1] + previousTreeLayer[i]));
                }
                count = treeLayer.Count;
                previousTreeLayer = treeLayer;
            }

            return treeLayer.Count == 1 ? treeLayer[0] : "";
        }
    }
}
