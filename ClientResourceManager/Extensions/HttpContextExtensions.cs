using System.Web;

namespace ClientResourceManager
{
    public static class HttpContextExtensions
    {
        public static ClientResourceRegistryBuilder ClientResources(this HttpContext context)
        {
            return new ClientResourceRegistryBuilder(new ClientResourceRegistry(context.Items));
        }

        public static ClientResourceRegistryBuilder ClientResources(this HttpContextBase context)
        {
            return new ClientResourceRegistryBuilder(new ClientResourceRegistry(context.Items));
        }

        internal static bool IsAjax(this HttpContextBase context)
        {
            var request = context.Request;

            return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

    }
}
