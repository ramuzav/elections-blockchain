using System;
using System.Collections.Generic;

namespace Blockchain
{
    public class Transaction
    {
        public string TransactionId { set; get; } // this is also the hash of the transaction.
        public string Sender { set; get; } // senders address/public key.
        public string Reciepient { set; get; } // Recipients address/public key.
        public double Value { set; get; }
        public byte[] Signature { set; get; } // this is to prevent anybody else from spending funds in our wallet.

        public List<TransactionInput> Inputs { set; get; }
        public List<TransactionOutput> Outputs { set; get; }

        private static int sequence = 0; // a rough count of how many transactions have been generated. 

        // Constructor: 
        public Transaction(string from, string to, double value, List<TransactionInput> inputs)
        {
            Sender = from;
            Reciepient = to;
            Value = value;
            Inputs = inputs;

            Outputs = new List<TransactionOutput>();
        }

        // This Calculates the transaction hash (which will be used as its Id)
        private string CalulateHash()
        {
            sequence++; //increase the sequence to avoid 2 identical transactions having the same hash
            return StringUtil.ApplySha256(
                    StringUtil.GetStringFromKey(Sender) +
                    StringUtil.GetStringFromKey(Reciepient) +
                    Value.ToString() + sequence);
        }

        //Signs all the data we dont wish to be tampered with.
        public void GenerateSignature(string privateKey)
        {
            var data = StringUtil.GetStringFromKey(Sender) + 
                       StringUtil.GetStringFromKey(Reciepient) + 
                       Value.ToString();

            Signature = StringUtil.SignData(privateKey, data);
        }

        //Verifies the data we signed hasnt been tampered with
        public bool VerifySignature()
        {
            var data = StringUtil.GetStringFromKey(Sender) + 
                       StringUtil.GetStringFromKey(Reciepient) + 
                       Value.ToString();

            return StringUtil.VerifyData(Sender, data, Signature);
        }

        //Returns true if new transaction could be created.	
        public bool ProcessTransaction()
        {

            if (VerifySignature() == false)
            {
                Console.WriteLine("#Transaction Signature failed to verify");
                return false;
            }

            //gather transaction inputs (Make sure they are unspent):
            foreach (var ti in Inputs)
            {
                ti.UTXO = ElectionsBlockchain.UTXOs[ti.TransactionOutputId];
            }

            //check if transaction is valid:
            if (GetInputsValue() < ElectionsBlockchain.MinimumTransaction)
            {
                Console.WriteLine("#Transaction Inputs to small: " + GetInputsValue());
                return false;
            }

            //generate transaction outputs:
            var leftOver = GetInputsValue() - Value; //get value of inputs then the left over change:
            TransactionId = CalulateHash();
            Outputs.Add(new TransactionOutput(Reciepient, Value, TransactionId)); //send value to recipient
            Outputs.Add(new TransactionOutput(Sender, leftOver, TransactionId)); //send the left over 'change' back to sender		

            //add outputs to Unspent list
            foreach (var to in Outputs)
            {
                ElectionsBlockchain.UTXOs.Add(to.Id, to);
            }

            //remove transaction inputs from UTXO lists as spent:
            foreach (var ti in Inputs)
            {
                if (ti.UTXO == null)
                    continue; //if Transaction can't be found skip it 
                ElectionsBlockchain.UTXOs.Remove(ti.UTXO.Id);
            }

            return true;
        }

        //returns sum of inputs(UTXOs) values
        public double GetInputsValue()
        {
            double total = 0;
            foreach (var ti in Inputs)
            {
                if (ti.UTXO == null)
                    continue; //if Transaction can't be found skip it 
                total += ti.UTXO.Value;
            }
            return total;
        }

        //returns sum of outputs:
        public double GetOutputsValue()
        {
            double total = 0;
            foreach (var to in Outputs)
            {
                total += to.Value;
            }
            return total;
        }
    }
}
