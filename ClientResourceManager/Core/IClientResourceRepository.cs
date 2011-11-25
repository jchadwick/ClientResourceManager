using System.Collections.Generic;

namespace ClientResourceManager.Core
{
    public interface IClientResourceRepository
    {
        IEnumerable<ClientResource> GetByKey(params string[] resourceKeys);
        string GetKey(IEnumerable<ClientResource> resources);
    }
}
