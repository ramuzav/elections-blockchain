using System;
using System.Collections.Generic;

namespace Blockchain
{
    public class Block
    {
        public string Hash { set; get; }
        public string PreviousHash { set; get; }
        public string MerkleRoot { set; get; }
        public List<Transaction> Transactions { set; get; } //our data will be a simple message.
        public long TimeStamp { private set; get; }
        public int Nonce { private set; get; }

        //Block Constructor.
        public Block(String previousHash) 
        {
            PreviousHash = previousHash;
            TimeStamp = new DateTime().TimeOfDay.Ticks;
            Hash = CalculateHash();

            Transactions = new List<Transaction>();
        }

        public string CalculateHash()
        {
            return StringUtil.ApplySha256(
                PreviousHash +
                TimeStamp.ToString() +
                Nonce.ToString() +
                MerkleRoot
                );
        }

        public void MineBlock(int difficulty)
        {
            MerkleRoot = StringUtil.GetMerkleRoot(Transactions);

            string target = new string(new char[difficulty]).Replace('\0', '0'); 

            while (!Hash.Substring(0, difficulty).Equals(target))
            {
                Nonce++;
                Hash = CalculateHash();
            }
            Console.WriteLine("Block Mined!!! : " + Hash);
        }

        public bool AddTransaction(Transaction transaction)
        {
            //process transaction and check if valid, unless block is genesis block then ignore.
            if (transaction == null)
                return false;

            if ((PreviousHash != "0"))
            {
                if ((transaction.ProcessTransaction() != true))
                {
                    Console.WriteLine("Transaction failed to process. Discarded.");
                    return false;
                }
            }

            Transactions.Add(transaction);

            Console.WriteLine("Transaction Successfully added to Block");

            return true;
        }
    }
}
