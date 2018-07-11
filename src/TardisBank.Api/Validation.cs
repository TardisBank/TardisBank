using E247.Fun;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class Validation
    {
        public static Result<RegisterRequest, TardisFault> Validate(
            this RegisterRequest registerRequest)
        {
            if(registerRequest == null)
            {
                return new TardisFault("request body missing");
            }
            if(string.IsNullOrEmpty(registerRequest.Email))
            {
                return new TardisFault("Email missing");
            }
            if(string.IsNullOrEmpty(registerRequest.Password))
            {
                return new TardisFault("Password is missing");
            }

            return registerRequest;
        }

        public static Result<LoginRequest, TardisFault> Validate(
            this LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                return new TardisFault("request body missing");
            }
            if(string.IsNullOrEmpty(loginRequest.Email))
            {
                return new TardisFault("Email missing");
            }
            if(string.IsNullOrEmpty(loginRequest.Password))
            {
                return new TardisFault("Password is missing");
            }

            return loginRequest;
        }

        public static Result<AccountRequest, TardisFault> Validate(
            this AccountRequest accountRequest)
        {
            if(accountRequest == null)
            {
                return new TardisFault("request body missing");
            }
            if(string.IsNullOrEmpty(accountRequest.AccountName))
            {
                return new TardisFault("AccountName is missing");
            }

            return accountRequest;
        }
    }
}