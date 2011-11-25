using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ClientResourceManager.Core
{
    public class ClientResourceRepository : IClientResourceRepository
    {
        public const char KeySeparator = ';';
        private readonly IEnumerable<ClientResource> _clientResources;


        public ClientResourceRepository(IEnumerable<ClientResource> clientResources)
        {
            Contract.Requires(clientResources != null);

            _clientResources = clientResources;
        }


        public IEnumerable<ClientResource> GetByKey(params string[] resourceKeys)
        {
            if (resourceKeys == null)
                return null;

            var keys = ResolveKeys(resourceKeys);

            var resources = _clientResources.Where(x => keys.Contains(x.Key.ToLower()));

            var unknownKeys = resourceKeys.Except(_clientResources.Select(x => x.Key.ToLower()));

            var localFilenames = unknownKeys.Where(x => x.IsLocalUrl());
            var localFileResources = localFilenames.Select(url => new ClientResource(url));

            resources = resources.Union(localFileResources);

            return resources.ToArray();
        }

        public string GetKey(IEnumerable<ClientResource> resources)
        {
            var keys = resources.Where(x => x != null).Select(x => x.Key);
            
            // TODO: Be more awesome (aliases!)

            return string.Join(KeySeparator.ToString(), keys);
        }

        protected internal IEnumerable<string> ResolveKeys(IEnumerable<string> resourceKeys)
        {
            if (resourceKeys == null)
                return null;

            return resourceKeys
                    .Where(id => id.HasValue())
                    .SelectMany(id => id.Split(KeySeparator))
                    .Select(id => id.ToLower());
        }
    }
}