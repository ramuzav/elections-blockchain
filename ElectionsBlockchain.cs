using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blockchain
{
    class ElectionsBlockchain
    {
        public static List<Block> blockchain = new List<Block>();
        public static int difficulty = 1;
        public static Wallet walletA;
        public static Wallet walletB;

        static void Main(string[] args)
        {
            //Create the new wallets
            walletA = new Wallet();
            walletB = new Wallet();
            //Test public and private keys
            Console.WriteLine("Private key:");
            Console.WriteLine(StringUtil.GetStringFromKey(walletA.PrivateKey));

            Console.WriteLine("Public key:");
            Console.WriteLine(StringUtil.GetStringFromKey(walletA.PublicKey));
            //Create a test transaction from WalletA to walletB 
            Transaction transaction = new Transaction(walletA.PublicKey, walletB.PublicKey, 5, null);
            transaction.GenerateSignature(walletA.PrivateKey);
            //Verify the signature works and verify it from the public key
            Console.WriteLine("Is signature verified");
            Console.WriteLine(transaction.VerifySignature());

            //String blockchainJson = JsonConvert.SerializeObject(blockchain);
            //Console.WriteLine(blockchainJson);
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
