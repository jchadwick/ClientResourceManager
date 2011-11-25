using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using ClientResourceManager.Configuration;
using ClientResourceManager.Util;

namespace ClientResourceManager
{
    public class ClientResourceManagerBuilder : IFluentInterface
    {
        private readonly ClientResourceManager _resourceManager;

        public IEnumerable<ClientResource> Resources
        {
            get { return _resourceManager.Resources; }
        }

        public IEnumerable<ClientResource> Scripts
        {
            get { return _resourceManager.Scripts; }
        }

        public IEnumerable<ClientResource> Stylesheets
        {
            get { return _resourceManager.Stylesheets; }
        }


        public ClientResourceManagerBuilder(ClientResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public ClientResourceManagerBuilder Include(string uri, Level level = null)
        {
            var resource = new ClientResource(uri) { Level = level };
            IncludeClientResource(resource);

            return this;
        }

        public ClientResourceManagerBuilder IncludeScript(string uri, Level level = null)
        {
            var resource = new ClientResource(uri, ClientResourceKind.Script) { Level = level };
            IncludeClientResource(resource);

            return this;
        }


        public ClientResourceManagerBuilder IncludeStylesheet(string uri, Level level = null)
        {
            var resource = new ClientResource(uri, ClientResourceKind.Stylesheet) { Level = level };
            IncludeClientResource(resource);

            return this;
        }


        public ClientResourceManagerBuilder IncludeEmbeddedResource<T>(string resourceName, ClientResourceKind?  kind = null, Level level = null)
        {
            return IncludeEmbeddedResource(typeof (T).Assembly, resourceName, kind, level);
        }

        public ClientResourceManagerBuilder IncludeEmbeddedResource(Assembly assembly, string resourceName, ClientResourceKind? kind = null, Level level = null)
        {
            return IncludeClientResource(new EmbeddedClientResource(assembly, resourceName, kind) { Level = level });
        }


        public ClientResourceManagerBuilder IncludeClientResource(ClientResource resource)
        {
            _resourceManager.Register(resource);

            return this;
        }


        public ClientResourceManagerBuilder OnDocumentReady(string scriptBlock)
        {
            _resourceManager.OnDocumentReady(scriptBlock);

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

            var resources = _resourceManager.Resources.Where(filter).ToArray();

            var stylesheets = resources.Where(x => x.Kind == ClientResourceKind.Stylesheet);
            foreach (var stylesheet in stylesheets)
            {
                var relativeUrl = GetUrl(stylesheet);
                writer.WriteLine("<link rel='stylesheet' type='text/css' href='{0}' />", relativeUrl);
            }

            var scripts = resources.Where(x => x.Kind == ClientResourceKind.Script);
            foreach (var script in scripts)
            {
                var relativeUrl = GetUrl(script);
                writer.WriteLine("<script type='text/javascript' src='{0}'></script>", relativeUrl);
            }
        }

        private static string GetUrl(ClientResource resource)
        {
            var url = resource.Url;

            if (Settings.Current.HandlerMode != HandlerMode.Disabled)
            {
                url = Handler.GenerateUrl(resource);
            }

            var relativeUrl = VirtualPathUtility.ToAbsolute(url);
            return relativeUrl;
        }

        protected internal virtual void RenderScriptStatements(TextWriter writer)
        {
            writer.WriteLine("<script type='text/javascript'>//<![CDATA[");
            writer.WriteLine("window.onload = function() {");
            foreach (var statement in _resourceManager.OnDocumentReadyStatements)
            {
                writer.Write(statement);

                if (!statement.EndsWith(";"))
                    writer.Write(";");
            }
            writer.WriteLine("\r\n};");
            writer.WriteLine("//]]>");
            writer.WriteLine("</script>");
        }
    }
}
