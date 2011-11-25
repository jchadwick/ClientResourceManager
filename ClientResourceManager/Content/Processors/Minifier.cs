using System.IO;
using ClientResourceManager.Configuration;
using ClientResourceManager.Util;

namespace ClientResourceManager.Content.Processors
{
    public class Minifier : IClientResourceContentFilter
    {
        private readonly Microsoft.Ajax.Utilities.Minifier _minifier;

        public Minifier()
        {
            _minifier = new Microsoft.Ajax.Utilities.Minifier();
        }

        public void Write(ClientResourceContent content, Stream output)
        {
            var isJavascript = content.ContentType == KnownMimeTypes.Javascript;
            var isStylesheet = content.ContentType == KnownMimeTypes.Stylesheet;

            var isMinifiable = (isJavascript || isStylesheet);

            if (Settings.Current.Minification == false || isMinifiable == false)
            {
                content.Write(output);
                return;
            }

            using (var contentStream = new MemoryStream())
            using (var contentReader = new StreamReader(contentStream))
            using (var outputWriter = new StreamWriter(output))
            {
                content.Write(contentStream);

                contentStream.Seek(0, SeekOrigin.Begin);

                var source = contentReader.ReadToEnd();

                string minified = source;

                if(isJavascript)
                    minified = _minifier.MinifyJavaScript(source);
                
                if(isStylesheet)
                    minified = _minifier.MinifyStyleSheet(source);

                outputWriter.Write(minified);
                
                outputWriter.Flush();
            }
        }
    }

    public interface IClientResourceContentFilter
    {
        void Write(ClientResourceContent content, Stream output);
    }
}
