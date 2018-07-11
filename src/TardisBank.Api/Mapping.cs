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
            => new AccountResponse
            {
                AccountName = account.AccountName
            };

        public static AccountResponseCollection ToDto(this IEnumerable<Account> accounts)
            => new AccountResponseCollection
            {
                Accounts = accounts.Select(x => x.ToDto()).ToArray()
            };
    }
}