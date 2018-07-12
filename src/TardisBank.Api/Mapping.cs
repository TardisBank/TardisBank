using System;
using System.Collections.Generic;
using System.Linq;
using E247.Fun;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class Mapping
    {
        public static Login ToModel(
            this RegisterRequest registerRequest)
            => new Login
            {
                Email = registerRequest.Email,
                PasswordHash = Password.HashPassword(registerRequest.Password)
            };

        public static RegisterResponse ToDto(
            this Login login)
            => new RegisterResponse();

        public static Account ToModel(this AccountRequest accountRequest, Login login)
            => new Account
            {
                LoginId = login.LoginId,
                AccountName = accountRequest.AccountName
            };

        public static AccountResponse ToDto(this Account account)
        {
            var accountResponse = new AccountResponse
            {
                AccountName = account.AccountName
            };
            accountResponse.AddLink(Rels.Self, $"/account/{account.AccountId}");
            accountResponse.AddLink(Rels.Transaction, $"/account/{account.AccountId}/transaction");
            accountResponse.AddLink(Rels.Schedule, $"/account/{account.AccountId}/schedule");
            return accountResponse;
        }

        public static AccountResponseCollection ToDto(this IEnumerable<Account> accounts)
            => new AccountResponseCollection
            {
                Accounts = accounts.Select(x => x.ToDto()).ToArray()
            };

        public static Transaction ToModel(
            this TransactionRequest transactionRequest,
            Account account,
            Transaction lastTransaction,
            DateTimeOffset transactionDate)
            => new Transaction
            {
                AccountId = account.AccountId,
                TransactionDate = transactionDate,
                Amount = transactionRequest.Amount
            }.CalculateBalance(lastTransaction);

        public static TransactionResponse ToDto(this Transaction transaction)
        {
            var transactionResponse = new TransactionResponse
            {
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                Balance = transaction.Balance
            };
            transactionResponse.AddLink(Rels.Self, 
                $"/account/{transaction.AccountId}/transaction/{transaction.TransactionId}");
            return transactionResponse;
        }

        public static TransactionResponseCollection ToDto(this IEnumerable<Transaction> transactions)
            => new TransactionResponseCollection
            {
                Transactions = transactions.Select(x => x.ToDto()).ToArray()
            };

        public static TimePeriod ToModel(this ScheduleTimePeriod scheduleTimePeriod)
        {
            switch(scheduleTimePeriod)
            {
                case ScheduleTimePeriod.day:
                    return TimePeriod.day;
                case ScheduleTimePeriod.week:
                    return TimePeriod.week;
                case ScheduleTimePeriod.month:
                    return TimePeriod.month;
                default:
                    throw new ApplicationException("Invalid ScheduleTimePeriod value");
            }
        }

        public static ScheduleTimePeriod ToDto(this TimePeriod timePeriod)
        {
            switch(timePeriod)
            {
                case TimePeriod.day:
                    return ScheduleTimePeriod.day;
                case TimePeriod.week:
                    return ScheduleTimePeriod.week;
                case TimePeriod.month:
                    return ScheduleTimePeriod.month;
                default:
                    throw new ApplicationException("Invalid TimePeriod value");
            }
        }

        public static Schedule ToModel(
            this ScheduleRequest scheduleRequest,
            Account account)
            => new Schedule
            {
                AccountId = account.AccountId,
                TimePeriod = scheduleRequest.TimePeriod.ToModel(),
                NextRun = scheduleRequest.NextRun,
                Amount = scheduleRequest.Amount
            };

        public static ScheduleResponse ToDto(this Schedule schedule)
        {
            var scheduleResponse = new ScheduleResponse
            {
                TimePeriod = schedule.TimePeriod.ToDto(),
                NextRun = schedule.NextRun,
                Amount = schedule.Amount
            };
            scheduleResponse.AddLink(Rels.Self, 
                $"/account/{schedule.AccountId}/schedule/{schedule.ScheduleId}");
            return scheduleResponse;
        }

        public static ScheduleResponseCollection ToDto(this IEnumerable<Schedule> schedules)
            => new ScheduleResponseCollection
            {
                Schedules = schedules.Select(x => x.ToDto()).ToArray()
            };
    }
}