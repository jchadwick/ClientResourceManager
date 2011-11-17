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
    }
}
