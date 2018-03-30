namespace Blockchain
{
    public class TransactionInput
    {
        public string TransactionOutputId { set;  get; } //Reference to TransactionOutputs -> transactionId
        public TransactionOutput UTXO { set; get; } //Contains the Unspent transaction output

        public TransactionInput(string transactionOutputId)
        {
            TransactionOutputId = transactionOutputId;
        }
    }
}
