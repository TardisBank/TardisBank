using System;

namespace TardisBank.Dto
{
    public class TransactionResponse : ResponseModelBase
    {
        public DateTimeOffset TransactionDate { get; set;}
        public decimal Amount { get; set;}
        public decimal Balance { get; set; }
    }
}