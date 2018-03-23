using System.Security.Cryptography;

namespace Blockchain
{
    public class Wallet
    {
        public string privateKey;
        public string publicKey;

        public Wallet()
        {
            GenerateKeyPair();
        }

        public void GenerateKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters rsaKeyInfo = rsa.ExportParameters(true);
        }
    }
}
