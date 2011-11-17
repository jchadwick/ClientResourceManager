using System.Web;

namespace ClientResourceManager.Extensions
{
    public static class HttpContextExtensions
    {
        public static ClientResourceRegistryBuilder ClientResources(this HttpContext context)
        {
            return ClientResources(new HttpContextWrapper(context));
        }

        public static ClientResourceRegistryBuilder ClientResources(this HttpContextBase context)
        {
            return new ClientResourceRegistryBuilder(new ClientResourceRegistry(context.Items));
        }
    }
}
