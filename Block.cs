using System;

namespace Blockchain
{
    public class Block
    {
        public string hash;
        public string previousHash;
        private string data; //our data will be a simple message.
        private long timeStamp; //as number of milliseconds since 1/1/1970.
        private int nonce;

        //Block Constructor.
        public Block(String data, String previousHash) 
        {
            this.data = data;
            this.previousHash = previousHash;
            this.timeStamp = new DateTime().TimeOfDay.Ticks;
            this.hash = CalculateHash();
        }

        public string CalculateHash()
        {
            return StringUtil.ApplySha256(previousHash + timeStamp.ToString() + data);
        }

        public void MineBlock(int difficulty)
        {
            string target = new string(new char[difficulty]).Replace('\0', '0'); 
            while (!hash.Substring(0, difficulty).equals(target))
            {
                nonce++;
                hash = calculateHash();
            }
            System.out.println("Block Mined!!! : " + hash);
        }
    }
}
