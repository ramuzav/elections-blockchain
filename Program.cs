using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blockchain
{
    class Program
    {
        public static List<Block> blockchain = new List<Block>(); 

        static void Main(string[] args)
        {
            blockchain.Add(new Block("Hi im the first block", "0"));		
            blockchain.Add(new Block("Yo im the second block", blockchain[blockchain.Count-1].hash)); 
            blockchain.Add(new Block("Hey im the third block", blockchain[blockchain.Count-1].hash));
            
            String blockchainJson = JsonConvert.SerializeObject(blockchain);		
            Console.WriteLine(blockchainJson);
        }

        public static Boolean IsChainValid()
        {
            Block currentBlock;
            Block previousBlock;

            //loop through blockchain to check hashes:
            for (int i = 1; i < blockchain.Count; i++)
            {
                currentBlock = blockchain[i];
                previousBlock = blockchain[i - 1];
                //compare registered hash and calculated hash:
                if (!currentBlock.hash.Equals(currentBlock.CalculateHash()))
                {
                    Console.WriteLine("Current Hashes not equal");
                    return false;
                }
                //compare previous hash and registered previous hash
                if (!previousBlock.hash.Equals(currentBlock.previousHash))
                {
                    Console.WriteLine("Previous Hashes not equal");
                    return false;
                }
            }
            return true;
        }
    }
}
