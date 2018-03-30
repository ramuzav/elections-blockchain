using System;

namespace Blockchain
{
    public class TransactionOutput
    {
        public string Id { get; set; }
        public string Reciepient { get; set; } //also known as the new owner of these coins.
        public float Value { get; set; } //the amount of coins they own
        public string ParentTransactionId { get; set; } //the id of the transaction this output was created in

        //Constructor
        public TransactionOutput(string reciepient, float value, String parentTransactionId)
        {
            Reciepient = reciepient;
            Value = value;
            ParentTransactionId = parentTransactionId;
            Id = StringUtil.ApplySha256(StringUtil.GetStringFromKey(reciepient) + value.ToString() + parentTransactionId);
        }

        //Check if coin belongs to you
        public bool isMine(string publicKey)
        {
            return (publicKey == Reciepient);
        }
    }
}
