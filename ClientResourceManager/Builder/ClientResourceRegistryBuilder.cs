using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace ClientResourceManager
{
    public class ClientResourceRegistryBuilder
    {
        private readonly ClientResourceRegistry _resourceRegistry;

        public IEnumerable<ClientResource> Resources
        {
            get { return _resourceRegistry.Resources; }
        }

        public IEnumerable<ClientResource> Scripts
        {
            get { return _resourceRegistry.Scripts; }
        }

        public IEnumerable<ClientResource> Stylesheets
        {
            get { return _resourceRegistry.Stylesheets; }
        }


        public ClientResourceRegistryBuilder(ClientResourceRegistry resourceRegistry)
        {
            _resourceRegistry = resourceRegistry;
        }

        public ClientResourceRegistryBuilder Include(string uri)
        {
            return Include(uri, null);
        }

        public ClientResourceRegistryBuilder Include(string uri, Level level)
        {
            var kind = ClientResourceRegistry.GuessResourceKind(uri);

            var resource = new ClientResource(uri) { Kind = kind, Level = level };
            IncludeClientResource(resource);

            return this;
        }

        public ClientResourceRegistryBuilder IncludeScript(string uri)
        {
            return IncludeScript(uri, null);
        }

        public ClientResourceRegistryBuilder IncludeScript(string uri, Level level)
        {
            var resource = new ClientResource(uri) { Kind = ClientResourceKind.Script, Level = level };
            IncludeClientResource(resource);

            return this;
        }


        public ClientResourceRegistryBuilder IncludeStylesheet(string uri)
        {
            return IncludeStylesheet(uri, null);
        }

        public ClientResourceRegistryBuilder IncludeStylesheet(string uri, Level level)
        {
            var resource = new ClientResource(uri) { Kind = ClientResourceKind.Stylesheet, Level = level };
            IncludeClientResource(resource);

            return this;
        }

        public ClientResourceRegistryBuilder IncludeClientResource(ClientResource resource)
        {
            _resourceRegistry.Register(resource);

            return this;
        }


        public IHtmlString Render()
        {
            return Render(x => (x.Level ?? Level.Loose) < Level.Global);
        }

        public IHtmlString RenderHead()
        {
            return Render(x => x.Level >= Level.Global);
        }

        protected IHtmlString Render(Func<ClientResource, bool> filter = null)
        {
            filter = filter ?? (x => true);

            var buffer = new StringBuilder();

            var stylesheets = _resourceRegistry.Stylesheets.Where(filter);
            foreach (var stylesheet in stylesheets)
            {
                var relativeUrl = VirtualPathUtility.ToAbsolute(stylesheet.Url);
                buffer.AppendFormat("<link rel='stylesheet' type='text/stylesheet' href='{0}' />", relativeUrl);
                buffer.AppendLine();
            }

            var scripts = _resourceRegistry.Scripts.Where(filter);
            foreach (var script in scripts)
            {
                var relativeUrl = VirtualPathUtility.ToAbsolute(script.Url);
                buffer.AppendFormat("<script type='text/javascript' src='{0}'></script>", relativeUrl);
                buffer.AppendLine();
            }

            return new HtmlString(buffer.ToString());
        }

        #region Hidden methods

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
