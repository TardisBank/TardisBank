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
            this Task<Result<TSuccess, TFailure>> input, 
            Func<TSuccess, Task> func)
        {
            var awaitedInput = await input;
            if(!awaitedInput.IsSuccessful) return awaitedInput;
            await func(awaitedInput.Success);
            return awaitedInput;
        }

        public async static Task<Result<TSuccess, TFailure>> RunAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> input, 
            Func<TSuccess, Task> func)
        {
            if(!input.IsSuccessful) return input;
            await func(input.Success);
            return input;
        }

        public async static Task<Result<TSuccess, TFailure>> Run<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> input, 
            Action<TSuccess> func)
        {
            var awaitedInput = await input;
            if(!awaitedInput.IsSuccessful) return awaitedInput;
            func(awaitedInput.Success);
            return awaitedInput;
        }

        // Task
        public async static Task<R> Map<T, R>(this Task<T> input, Func<T, R> func)
        {
            var awaitedInput = await input;
            return func(awaitedInput);
        }

        public static R Pipe<T, R>(this T input, Func<T, R> func)
            => func(input);
    }
}