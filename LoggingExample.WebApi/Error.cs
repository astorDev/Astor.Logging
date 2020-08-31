using System;
using System.Collections;
using System.Linq;
using System.Net;

namespace LoggingExample.WebApi
{
    public class Error
    {
        public HttpStatusCode Code { get; set; }

        public string Reason { get; set; }

        public object Details { get; set; }

        public static Error Unknown => new Error
        {
            Code = HttpStatusCode.InternalServerError,
            Reason = "Unknown"
        };

        public static Error Interpret(Exception exception, bool showDetails)
        {
            var error = interpret(exception);
            if (showDetails)
            {
                error.Details = new
                {
                    exceptionMessage = exception.Message,
                    exceptionData = any(exception.Data) ? exception.Data : null,
                    innerExceptionMessage = exception.InnerException?.Message,
                    stackTrace = exception.StackTrace
                };
            }

            return error;
        }

        private static Error interpret(Exception exception)
        {
            return Unknown;
        }

        private static bool any(IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}