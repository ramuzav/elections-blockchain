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
    }
}
