using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Blockchain
{
    public class Wallet
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public Dictionary<string, TransactionOutput> UTXOs { get; set; } //only UTXOs owned by this wallet.

        public Wallet()
        {
            GenerateKeyPair();
            UTXOs = new Dictionary<string, TransactionOutput>();
        }

        public void GenerateKeyPair()
        {
            CspParameters param = new CspParameters(1);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param);
            PrivateKey = rsa.ToXmlStringInternal(true);
            PublicKey = rsa.ToXmlStringInternal(false);
        }

        public double GetBalance()
        {
            double total = 0;
            foreach (var item in ElectionsBlockchain.UTXOs)
            {
                var UTXO = item.Value;
                if (UTXO.IsMine(PublicKey))
                { 
                    UTXOs.Add(UTXO.Id, UTXO);
                    total += UTXO.Value;
                }
            }
            return total;
        }

        public Transaction SendFunds(string _recipient, double value)
        {
            if (GetBalance() < value)
            { 
                Console.WriteLine("#Not Enough funds to send transaction. Transaction Discarded.");
                return null;
            }

            var inputs = new List<TransactionInput>();

            double total = 0;
            foreach (var item in UTXOs)
            {
                var UTXO = item.Value;
                total += UTXO.Value;
                inputs.Add(new TransactionInput(UTXO.Id));
                if (total > value)
                    break;
            }

            Transaction newTransaction = new Transaction(PublicKey, _recipient, value, inputs);
            newTransaction.GenerateSignature(PrivateKey);

            foreach (var input in inputs)
            {
                UTXOs.Remove(input.TransactionOutputId);
            }
            return newTransaction;
        }
    }
}
