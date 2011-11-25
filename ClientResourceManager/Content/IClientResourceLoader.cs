using System.Collections.Generic;

namespace ClientResourceManager.Content
{
    public interface IClientResourceLoader
    {
        ClientResourceContent Load(IEnumerable<ClientResource> resources);
    }
}