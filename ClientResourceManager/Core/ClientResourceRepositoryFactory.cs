using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace ClientResourceManager.Core
{
    public class ClientResourceRepositoryFactory
    {
        public ClientResourceRepository Create()
        {
            IEnumerable<ClientResource> embeddedResources =
                from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                from resourceName in assembly.GetManifestResourceNames()
                select new EmbeddedClientResource(assembly, resourceName);

            return new ClientResourceRepository(embeddedResources);
        }
    }
}