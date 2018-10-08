namespace TardisBank.Dto
{
    public class TransactionResponseCollection : ResponseModelBase
    {
        public TransactionResponse[] Transactions { get; set; }
    }
}