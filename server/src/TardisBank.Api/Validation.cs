using System;
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
                return new TardisFault("Request body missing");
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
                return new TardisFault("Request body missing");
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
                return new TardisFault("Request body missing");
            }
            if(string.IsNullOrEmpty(accountRequest.AccountName))
            {
                return new TardisFault("AccountName is missing");
            }

            return accountRequest;
        }

        public static Result<ScheduleRequest, TardisFault> Validate(
            this ScheduleRequest scheduleRequest)
        {
            if(scheduleRequest == null)
            {
                return new TardisFault("Request body is missing");
            }
            if(scheduleRequest.Amount < 0)
            {
                return new TardisFault("Schedule amount can not be less than zero");
            }
            if(scheduleRequest.NextRun < DateTimeOffset.Now)
            {
                return new TardisFault("Next run cannot be in the past.");
            }

            return scheduleRequest;
        }

        public static Result<ChangePasswordRequest, TardisFault> Validate(
            this ChangePasswordRequest changePasswordRequest)
        {
            if(changePasswordRequest == null)
            {
                return new TardisFault("Request body is missing");
            }

            if(string.IsNullOrEmpty(changePasswordRequest.OldPassword))
            {
                return new TardisFault("OldPassword is missing");
            }

            if(string.IsNullOrEmpty(changePasswordRequest.NewPassword))
            {
                return new TardisFault("NewPassword is missing");
            }

            return changePasswordRequest;
        }
    }
}