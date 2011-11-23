using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ClientResourceManager
{
    public class ClientResourceRegistry
    {
        public const string DictionaryKey = "ClientResourceRegistry.Resources";

        private readonly IList<ClientResource> _resources;
        private readonly IList<string> _onDocumentReadyStatements = new List<string>();

        public IEnumerable<string> OnDocumentReadyStatements
        {
            get { return _onDocumentReadyStatements; }
        }

        public IEnumerable<ClientResource> Resources
        {
            get
            {
                return from resource in _resources
                       orderby resource.Level descending, resource.Ordinal
                       group resource by resource.Url into groups
                       let maxLevel = groups.Max(x => x.Level)
                       select groups.FirstOrDefault(x => x.Level == maxLevel);
            }
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
            _resources = new List<ClientResource>();
        }

        public ClientResourceRegistry(IDictionary container)
        {
            Contract.Requires(container != null);

            if (container[DictionaryKey] == null)
                container[DictionaryKey] = new List<ClientResource>();

            _resources = (IList<ClientResource>)container[DictionaryKey];
        }

        public ClientResourceRegistry(IEnumerable<ClientResource> clientScripts)
        {
            Contract.Requires(clientScripts != null);

            _resources = clientScripts as IList<ClientResource>;

            if(_resources == null)
                _resources = new List<ClientResource>(clientScripts);
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

        public void OnDocumentReady(string scriptBlock)
        {
            _onDocumentReadyStatements.Add(scriptBlock);
        }


        public static ClientResourceKind GuessResourceKind(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                return default(ClientResourceKind);

            if (uri.ToLowerInvariant().Contains(".js"))
                return ClientResourceKind.Script;

            if (uri.ToLowerInvariant().Contains(".css"))
                return ClientResourceKind.Stylesheet;

            return ClientResourceKind.Content;
        }
    }
}
