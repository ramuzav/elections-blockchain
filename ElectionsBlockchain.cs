using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blockchain
{
    class ElectionsBlockchain
    {
        public static List<Block> blockchain = new List<Block>();
        public static int difficulty = 1;

        static void Main(string[] args)
        {
            blockchain.Add(new Block("Hi im the first block", "0"));
            Console.WriteLine("Trying to Mine block 1... ");
            blockchain[0].MineBlock(difficulty);

            blockchain.Add(new Block("Yo im the second block", blockchain[blockchain.Count - 1].Hash));
            Console.WriteLine("Trying to Mine block 2... ");
            blockchain[1].MineBlock(difficulty);

            blockchain.Add(new Block("Hey im the third block", blockchain[blockchain.Count - 1].Hash));
            Console.WriteLine("Trying to Mine block 3... ");
            blockchain[2].MineBlock(difficulty);

            Console.WriteLine("\nBlockchain is Valid: " + IsChainValid());

            String blockchainJson = JsonConvert.SerializeObject(blockchain);
            Console.WriteLine("\nThe block chain: ");
            Console.WriteLine(blockchainJson);
        }

        public static Boolean IsChainValid()
        {
            Block currentBlock;
            Block previousBlock;
            var hashTarget = new String(new char[difficulty]).Replace('\0', '0');

            for (int i = 1; i < blockchain.Count; i++)
            {
                currentBlock = blockchain[i];
                previousBlock = blockchain[i - 1];
                if (!currentBlock.Hash.Equals(currentBlock.CalculateHash()))
                {
                    Console.WriteLine("Current Hashes not equal");
                    return false;
                }
                if (!previousBlock.Hash.Equals(currentBlock.PreviousHash))
                {
                    Console.WriteLine("Previous Hashes not equal");
                    return false;
                }
                if (!currentBlock.Hash.Substring(0, difficulty).Equals(hashTarget))
                {
                    Console.WriteLine("This block hasn't been mined");
                    return false;
                }
            }
            return true;
        }
    }
}
