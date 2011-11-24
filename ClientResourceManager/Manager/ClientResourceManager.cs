using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ClientResourceManager
{
    /// <summary>
    /// The Manager of client resources for an individual request
    /// </summary>
    public class ClientResourceManager
    {
        public const string ResourcesDictionaryKey = "ClientResourceManager.Resources";
        public const string DocumentReadyStatementsDictionaryKey = "ClientResourceManager.DocumentReadyStatements";

        private readonly IList<ClientResource> _resources;
        private readonly IList<string> _onDocumentReadyStatements;

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


        public ClientResourceManager()
        {
            _resources = new List<ClientResource>();
        }

        public ClientResourceManager(IDictionary container)
        {
            Contract.Requires(container != null);

            container[ResourcesDictionaryKey] = _resources =
                container[ResourcesDictionaryKey] as List<ClientResource> ?? new List<ClientResource>();

            container[DocumentReadyStatementsDictionaryKey] = _onDocumentReadyStatements = 
                container[DocumentReadyStatementsDictionaryKey] as List<string> ?? new List<string>();
        }

        public ClientResourceManager(IEnumerable<ClientResource> clientScripts, IEnumerable<string> statements)
        {
            Contract.Requires(clientScripts != null);

            _resources = clientScripts as IList<ClientResource> 
                ?? new List<ClientResource>(clientScripts ?? Enumerable.Empty<ClientResource>());

            _onDocumentReadyStatements = statements as IList<string>
                ?? new List<string>(statements ?? Enumerable.Empty<string>());
        }


        public void Register(string key, ClientResourceKind? kind = null, Level level = null)
        {
            var resource = new ClientResource(key, kind) { Level = Level.Loose };

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
    }
}
