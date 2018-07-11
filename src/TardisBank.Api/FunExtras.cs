using System.Net;
using E247.Fun;

namespace TardisBank.Api
{
    public static class FunExtras
    {
        public static Result<T, TardisFault> ToTardisResult<T>(this Maybe<T> maybe, string message)
            => maybe.ToResult(() => new TardisFault(message));

        public static Result<T, TardisFault> ToTardisResult<T>(this Maybe<T> maybe, HttpStatusCode httpStatusCode, string message)
            => maybe.ToResult(() => new TardisFault(httpStatusCode, message));
    }
}