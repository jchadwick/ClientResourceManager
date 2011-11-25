using System;
using System.Net;
using System.Web;

namespace ClientResourceManager
{
    public static class HttpContextExtensions
    {
        public static ClientResourceManagerBuilder ClientResources(this HttpContext context)
        {
            return new ClientResourceManagerBuilder(new ClientResourceManager(context.Items));
        }

        public static ClientResourceManagerBuilder ClientResources(this HttpContextBase context)
        {
            return new ClientResourceManagerBuilder(new ClientResourceManager(context.Items));
        }


        public static bool IsAjaxRequest(this HttpContextBase context)
        {
            var request = context.Request;

            return (request["X-Requested-With"] == "XMLHttpRequest") 
                || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        public static bool HasBeenModifiedSince(this HttpContextBase context, DateTime? lastModifiedDate)
        {
            var header = context.Request.Headers["If-Modified-Since"];

            DateTime tempDate;
            if (header != null && lastModifiedDate != null && DateTime.TryParse(header, out tempDate))
            {
                // Don't compare milliseconds because they often produce false results
                var modifiedSinceUtc = NormalizeToSeconds(tempDate.ToUniversalTime());
                var lastModifiedUtc = NormalizeToSeconds(lastModifiedDate.Value.ToUniversalTime());

                return lastModifiedUtc > modifiedSinceUtc;
            }

            return true;
        }

        private static DateTime NormalizeToSeconds(DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day,
                                source.Hour, source.Minute, source.Second,
                                source.Kind);
        }

        public static void SetStatusCode(this HttpContextBase context, HttpStatusCode statusCode)
        {
            SetStatusCode(context, statusCode, null);
        }

        public static void SetStatusCode(this HttpContextBase context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;

            if (message.HasValue())
                context.Response.StatusDescription = message;
        }
    }
}
