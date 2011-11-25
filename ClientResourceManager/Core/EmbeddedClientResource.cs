using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web;

namespace ClientResourceManager
{
    public class EmbeddedClientResource : ClientResource
    {
        public Assembly Assembly { get; private set; }

        public string ResourceName { get; private set; }

        public override string Key
        {
            get { return ResourceName; }
        }

        public EmbeddedClientResource(Assembly assembly, string resourceName)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(resourceName.HasValue());

            Assembly = assembly;
            ResourceName = resourceName;
        }

        public EmbeddedClientResource(Assembly assembly, string resourceName, ClientResourceKind? kind)
            : this(assembly, resourceName)
        {
            if(kind != null)
                Kind = kind.Value;
        }


        protected override string BuildUrl()
        {
            var url = Assembly.GetWebResourceUrl(ResourceName);
            return VirtualPathUtility.ToAbsolute(url);
        }

        protected override ClientResourceKind GuessResourceKind()
        {
            return GuessResourceKind(ResourceName);
        }
    }
}