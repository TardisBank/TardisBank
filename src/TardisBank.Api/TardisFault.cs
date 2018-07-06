using System;
using System.Net;

namespace TardisBank.Api
{
    public struct TardisFault
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Message { get; }

        public TardisFault(HttpStatusCode httpStatusCode, string message)
        {
            HttpStatusCode = httpStatusCode;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public TardisFault(string message) : this(HttpStatusCode.BadRequest, message)
        {}
    }
}