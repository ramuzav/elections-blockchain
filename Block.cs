using System;

namespace Blockchain
{
    public class Block
    {
        public string Hash { set; get; }
        public string PreviousHash { set; get; }
        public string Data { private set; get; }
        public long TimeStamp { private set; get; }
        public int Nonce { private set; get; }

        //Block Constructor.
        public Block(String data, String previousHash) 
        {
            this.Data = data;
            this.PreviousHash = previousHash;
            this.TimeStamp = new DateTime().TimeOfDay.Ticks;
            this.Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            return StringUtil.ApplySha256(
                PreviousHash +
                TimeStamp.ToString() +
                Nonce.ToString() +
                Data
                );
        }

        public void MineBlock(int difficulty)
        {
            string target = new string(new char[difficulty]).Replace('\0', '0'); 
            while (!Hash.Substring(0, difficulty).Equals(target))
            {
                Nonce++;
                Hash = CalculateHash();
            }
            Console.WriteLine("Block Mined!!! : " + Hash);
        }
    }
}
