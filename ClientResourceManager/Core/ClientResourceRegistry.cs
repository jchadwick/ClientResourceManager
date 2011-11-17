using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ClientResourceManager
{
    public class ClientResourceRegistry
    {
        public const string DictionaryKey = "ClientResourceRegistry.Resources";

        private readonly ISet<ClientResource> _resources;

        public IEnumerable<ClientResource> Resources
        {
            get { return _resources; }
        }

        public IEnumerable<ClientResource> Scripts
        {
            get
            {
                return _resources.Where(x => x.Kind == ClientResourceKind.Script);
            }
        }

        public IEnumerable<ClientResource> Stylesheets
        {
            get { return _resources.Where(x => x.Kind == ClientResourceKind.Stylesheet); }
        }


        public ClientResourceRegistry()
        {
            _resources = new SortedSet<ClientResource>();
        }

        public ClientResourceRegistry(IDictionary container)
        {
            Contract.Requires(container != null);

            if (container[DictionaryKey] == null)
                container[DictionaryKey] = new SortedSet<ClientResource>();

            _resources = (ISet<ClientResource>)container[DictionaryKey];
        }

        public ClientResourceRegistry(IEnumerable<ClientResource> clientScripts)
        {
            Contract.Requires(clientScripts != null);

            _resources = clientScripts as ISet<ClientResource>;

            if(_resources == null)
                _resources = new SortedSet<ClientResource>(clientScripts);
        }

        public void Register(string uri, ClientResourceKind? kind = null, int? level = 0)
        {
            var resource = new ClientResource(uri) { Level = Level.Loose };

            if (kind != null)
                resource.Kind = GuessResourceKind(uri);

            Register(resource);
        }

        public void Register(ClientResource resource)
        {
            if(!_resources.Contains(resource))
                _resources.Add(resource);
        }


        public static ClientResourceKind GuessResourceKind(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return default(ClientResourceKind);

            if (uri.ToLowerInvariant().Contains(".js"))
                return ClientResourceKind.Script;

            if (uri.ToLowerInvariant().Contains(".css"))
                return ClientResourceKind.Stylesheet;

            return ClientResourceKind.Content;
        }
    }
}
