using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web;

namespace ClientResourceManager
{
    public class EmbeddedClientResource : ClientResource
    {
        public Assembly Assembly { get; private set; }

        public string ResourceName { get; private set; }


        public EmbeddedClientResource(Assembly assembly, string resourceName)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(resourceName.HasValue());

            Assembly = assembly;
            ResourceName = resourceName;
        }

        public EmbeddedClientResource(Assembly assembly, string resourceName, ClientResourceKind kind)
            : this(assembly, resourceName)
        {
            Kind = kind;
        }


        protected override string BuildUrl()
        {
            var url = Assembly.GetWebResourceUrl(ResourceName);
            return VirtualPathUtility.ToAbsolute(url);
        }
    }
}