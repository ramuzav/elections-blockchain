using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blockchain
{
    class ElectionsBlockchain
    {
        public static List<Block> blockchain = new List<Block>();

        public static Dictionary<string, TransactionOutput> UTXOs = new Dictionary<string, TransactionOutput>(); //list of all unspent transactions. 
        public static double MinimumTransaction = 0.1f;
        public static int difficulty = 1;
        public static Wallet walletA;
        public static Wallet walletB;
        public static Transaction genesisTransaction;

        static void Main(string[] args)
        {
            //Create wallets:
            walletA = new Wallet();
            walletB = new Wallet();
            Wallet coinbase = new Wallet();

            //create genesis transaction, which sends 100 NoobCoin to walletA: 
            genesisTransaction = new Transaction(coinbase.PublicKey, walletA.PublicKey, 100f, null);
            genesisTransaction.GenerateSignature(coinbase.PrivateKey);   //manually sign the genesis transaction	
            genesisTransaction.TransactionId = "0"; //manually set the transaction id
            genesisTransaction.Outputs.Add(new TransactionOutput(genesisTransaction.Reciepient, 
                                                                 genesisTransaction.Value, 
                                                                 genesisTransaction.TransactionId)); //manually add the Transactions Output
            UTXOs.Add(genesisTransaction.Outputs[0].Id, genesisTransaction.Outputs[0]); //its important to store our first transaction in the UTXOs list.

            Console.WriteLine("Creating and Mining Genesis block... ");
            Block genesis = new Block("0");
            genesis.AddTransaction(genesisTransaction);
            AddBlock(genesis);

            //testing
            Block block1 = new Block(genesis.Hash);
            Console.WriteLine("\nWalletA's balance is: " + walletA.GetBalance());
            Console.WriteLine("\nWalletA is Attempting to send funds (40) to WalletB...");
            block1.AddTransaction(walletA.SendFunds(walletB.PublicKey, 40f));
            AddBlock(block1);
            Console.WriteLine("\nWalletA's balance is: " + walletA.GetBalance());
            Console.WriteLine("WalletB's balance is: " + walletB.GetBalance());

            Block block2 = new Block(block1.Hash);
            Console.WriteLine("\nWalletA Attempting to send more funds (1000) than it has...");
            block2.AddTransaction(walletA.SendFunds(walletB.PublicKey, 1000f));
            AddBlock(block2);
            Console.WriteLine("\nWalletA's balance is: " + walletA.GetBalance());
            Console.WriteLine("WalletB's balance is: " + walletB.GetBalance());

            Block block3 = new Block(block2.Hash);
            Console.WriteLine("\nWalletB is Attempting to send funds (20) to WalletA...");
            block3.AddTransaction(walletB.SendFunds(walletA.PublicKey, 20));
            Console.WriteLine("\nWalletA's balance is: " + walletA.GetBalance());
            Console.WriteLine("WalletB's balance is: " + walletB.GetBalance());

            IsChainValid();

            //String blockchainJson = JsonConvert.SerializeObject(blockchain);
            //Console.WriteLine(blockchainJson);
        }

        public static Boolean IsChainValid()
        {
            Block currentBlock;
            Block previousBlock;
            var hashTarget = new string(new char[difficulty]).Replace('\0', '0');
            Dictionary<string, TransactionOutput> tempUTXOs = new Dictionary<string, TransactionOutput>();
            tempUTXOs.Add(genesisTransaction.Outputs[0].Id, genesisTransaction.Outputs[0]);

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

                TransactionOutput tempOutput;
                for (int t = 0; t < currentBlock.Transactions.Count; t++)
                {
                    var currentTransaction = currentBlock.Transactions[t];

                    if (!currentTransaction.VerifySignature())
                    {
                        Console.WriteLine("#Signature on Transaction(" + t + ") is Invalid");
                        return false;
                    }
                    if (currentTransaction.GetInputsValue() != currentTransaction.GetOutputsValue())
                    {
                        Console.WriteLine("#Inputs are note equal to outputs on Transaction(" + t + ")");
                        return false;
                    }

                    foreach (var input in currentTransaction.Inputs)
                    {
                        tempOutput = tempUTXOs[input.TransactionOutputId];

                        if (tempOutput == null)
                        {
                            Console.WriteLine("#Referenced input on Transaction(" + t + ") is Missing");
                            return false;
                        }

                        if (input.UTXO.Value != tempOutput.Value)
                        {
                            Console.WriteLine("#Referenced input Transaction(" + t + ") value is Invalid");
                            return false;
                        }

                        tempUTXOs.Remove(input.TransactionOutputId);
                    }

                    foreach (var output in currentTransaction.Outputs)
                    {
                        tempUTXOs.Add(output.Id, output);
                    }

                    if (currentTransaction.Outputs[0].Reciepient != currentTransaction.Reciepient)
                    {
                        Console.WriteLine("#Transaction(" + t + ") output reciepient is not who it should be");
                        return false;
                    }
                    if (currentTransaction.Outputs[1].Reciepient != currentTransaction.Sender)
                    {
                        Console.WriteLine("#Transaction(" + t + ") output 'change' is not sender.");
                        return false;
                    }

                }

            }
            return true;
        }

        public static void AddBlock(Block newBlock)
        {
            newBlock.MineBlock(difficulty);
            blockchain.Add(newBlock);
        }
    }
}
