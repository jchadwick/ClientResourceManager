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


        internal static bool IsAjaxRequest(this HttpContextBase context)
        {
            var request = context.Request;

            return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

        internal static void SetStatusCode(this HttpResponseBase response, HttpStatusCode statusCode)
        {
            response.StatusCode = (int)statusCode;
        }
    }
}
