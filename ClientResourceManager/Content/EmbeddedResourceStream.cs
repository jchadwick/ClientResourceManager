using System;
using System.IO;
using System.Reflection;

namespace ClientResourceManager.Content
{
    public class EmbeddedResourceStream : ClientResourceStream
    {
        private readonly DateTime? _lastAssemblyWriteTimeUtc;

        public Assembly Assembly { get; private set; }

        public override DateTime? LastModified
        {
            get { return _lastAssemblyWriteTimeUtc; }
            set { /* Readonly */ }
        }

        public string ResourceName { get; private set; }

        protected override Lazy<Stream> Stream
        {
            get { return _stream = _stream ?? GetLazyStream(Assembly, ResourceName); }
        }
        private Lazy<Stream> _stream;


        public EmbeddedResourceStream(Assembly assembly, string resourceName, string contentType)
            : base(contentType)
        {
            Assembly = assembly;
            ResourceName = resourceName;

            if (assembly != null && assembly.Location.HasValue())
            {
                _lastAssemblyWriteTimeUtc = File.GetLastWriteTimeUtc(assembly.Location);
            }
        }


        private static Lazy<Stream> GetLazyStream(Assembly assembly, string resourceName)
        {
            if (assembly == null)
                throw new ApplicationException("Assembly not specified");

            if(resourceName.IsNullOrWhiteSpace())
                throw new ApplicationException("ResourceName not specified");

            return new Lazy<Stream>(() => assembly.GetManifestResourceStream(resourceName));
        }
    }
}