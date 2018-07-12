using System;

namespace TardisBank.Api
{
    public class Login
    {
        public int LoginId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public int LoginId { get; set; }
        public string AccountName { get; set; }
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTimeOffset TransactionDate { get; set;}
        public decimal Amount { get; set;}
        public decimal Balance { get; set; }

        public Transaction CalculateBalance(Transaction lastTransaction)
        {
            Balance = lastTransaction.Balance + Amount;
            return this;
        }
    }

    public class Schedule
    {
        public int ScheduleId { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public DateTimeOffset NextRun { get; set; }
    }

    public enum TimePeriod
    {
        Day,
        Week,
        Month
    }

    public class Token
    {
        public Login Login { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}