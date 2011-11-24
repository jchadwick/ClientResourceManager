using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using ClientResourceManager.Plumbing;

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
            var resource = new ClientResource(uri) { Level = level };
            IncludeClientResource(resource);

            return this;
        }

        public ClientResourceRegistryBuilder IncludeScript(string uri)
        {
            return IncludeScript(uri, null);
        }

        public ClientResourceRegistryBuilder IncludeScript(string uri, Level level)
        {
            var resource = new ClientResource(uri, ClientResourceKind.Script) { Level = level };
            IncludeClientResource(resource);

            return this;
        }


        public ClientResourceRegistryBuilder IncludeStylesheet(string uri)
        {
            return IncludeStylesheet(uri, null);
        }

        public ClientResourceRegistryBuilder IncludeStylesheet(string uri, Level level)
        {
            var resource = new ClientResource(uri, ClientResourceKind.Stylesheet) { Level = level };
            IncludeClientResource(resource);

            return this;
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource<T>(string resourceName)
        {
            return IncludeEmbeddedResource(typeof(T).Assembly, resourceName);
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource<T>(string resourceName, ClientResourceKind kind)
        {
            return IncludeEmbeddedResource(typeof(T).Assembly, resourceName, kind);
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource<T>(string resourceName, ClientResourceKind kind, Level level)
        {
            return IncludeEmbeddedResource(typeof (T).Assembly, resourceName, kind, level);
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource(Assembly assembly, string resourceName)
        {
            return IncludeClientResource(new EmbeddedClientResource(assembly, resourceName));
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource(Assembly assembly, string resourceName, ClientResourceKind kind)
        {
            return IncludeClientResource(new EmbeddedClientResource(assembly, resourceName, kind));
        }

        public ClientResourceRegistryBuilder IncludeEmbeddedResource(Assembly assembly, string resourceName, ClientResourceKind kind, Level level)
        {
            return IncludeClientResource(new EmbeddedClientResource(assembly, resourceName, kind) {Level = level});
        }

        public ClientResourceRegistryBuilder IncludeClientResource(ClientResource resource)
        {
            _resourceRegistry.Register(resource);

            return this;
        }

        public ClientResourceRegistryBuilder OnDocumentReady(string scriptBlock)
        {
            _resourceRegistry.OnDocumentReady(scriptBlock);

            return this;
        }

        public IHtmlString Render()
        {
            using (var writer = new HtmlStringWriter())
            {
                Render(writer, x => (x.Level ?? Level.Loose) < Level.Global);
                RenderScriptStatements(writer);

                return writer;
            }
        }

        public IHtmlString RenderHead()
        {
            using (var writer = new HtmlStringWriter())
            {
                Render(writer, x => x.Level >= Level.Global);
                return writer;
            }
        }

        protected internal void Render(TextWriter writer, Func<ClientResource, bool> filter = null)
        {
            filter = filter ?? (x => true);

            var resources = _resourceRegistry.Resources.Where(filter).ToArray();

            var stylesheets = resources.Where(x => x.Kind == ClientResourceKind.Stylesheet);
            foreach (var stylesheet in stylesheets)
            {
                var relativeUrl = ResolveUrlAttribute(stylesheet.Url);
                writer.WriteLine("<link rel='stylesheet' type='text/stylesheet' href='{0}' />", relativeUrl);
            }

            var scripts = resources.Where(x => x.Kind == ClientResourceKind.Script);
            foreach (var script in scripts)
            {
                var relativeUrl = ResolveUrlAttribute(script.Url);
                writer.WriteLine("<script type='text/javascript' src='{0}'></script>", relativeUrl);
            }
        }

        protected virtual void RenderScriptStatements(TextWriter writer)
        {
            writer.WriteLine("<script type='text/javascript'>//<![CDATA[");
            writer.WriteLine("window.onload = function() {");
            foreach (var statement in _resourceRegistry.OnDocumentReadyStatements)
            {
                writer.Write(statement);

                if (!statement.EndsWith(";"))
                    writer.Write(";");
            }
            writer.WriteLine("\r\n};");
            writer.WriteLine("//]]>");
            writer.WriteLine("</script>");
        }

        private string ResolveUrlAttribute(string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
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
