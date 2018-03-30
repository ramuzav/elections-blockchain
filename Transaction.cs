using System.Collections.Generic;

namespace Blockchain
{
    public class Transaction
    {
        public string TransactionId { set; get; } // this is also the hash of the transaction.
        public string Sender { set; get; } // senders address/public key.
        public string Reciepient { set; get; } // Recipients address/public key.
        public float Value { set; get; }
        public byte[] Signature { set; get; } // this is to prevent anybody else from spending funds in our wallet.

        public List<TransactionInput> inputs = new List<TransactionInput>();
        public List<TransactionOutput> outputs = new List<TransactionOutput>();

        private static int sequence = 0; // a rough count of how many transactions have been generated. 

        // Constructor: 
        public Transaction(string from, string to, float value, List<TransactionInput> inputs)
        {
            this.Sender = from;
            this.Reciepient = to;
            this.Value = value;
            this.inputs = inputs;
        }

        // This Calculates the transaction hash (which will be used as its Id)
        private string CalulateHash()
        {
            sequence++; //increase the sequence to avoid 2 identical transactions having the same hash
            return StringUtil.ApplySha256(
                    StringUtil.GetStringFromKey(Sender) +
                    StringUtil.GetStringFromKey(Reciepient) +
                    Value.ToString() + sequence
                    );
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
    }
}
