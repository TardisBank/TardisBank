using System;
using System.Net;
using System.Threading.Tasks;
using E247.Fun;

namespace TardisBank.Api
{
    public static class FunExtras
    {
        public static Result<T, TardisFault> ToTardisResult<T>(this Maybe<T> maybe, string message)
            => maybe.ToResult(() => new TardisFault(message));

        public static Result<T, TardisFault> ToTardisResult<T>(this Maybe<T> maybe, HttpStatusCode httpStatusCode, string message)
            => maybe.ToResult(() => new TardisFault(httpStatusCode, message));

        public async static Task<Result<T, TardisFault>> ToTardisResult<T>(
            this Task<Maybe<T>> asyncMaybe, 
            HttpStatusCode httpStatusCode, 
            string message)
        {
            var maybe = await asyncMaybe;
            return maybe.ToTardisResult(message);
        }

        public async static Task<Result<TSuccess, TFailure>> RunAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> input, 
            Func<TSuccess, Task> func)
        {
            if(!input.IsSuccessful) return input;
            await func(input.Success);
            return input;
        }
    }
}