using System.Security.Cryptography;

namespace Blockchain
{
    public class Wallet
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public Wallet()
        {
            GenerateKeyPair();
        }

        public void GenerateKeyPair()
        {
            CspParameters param = new CspParameters(1);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param);
            // this hold the private key 
            PrivateKey = rsa.ToXmlString(true);
            // this hold the public key 
            PublicKey = rsa.ToXmlString(false);
        }
    }
}
