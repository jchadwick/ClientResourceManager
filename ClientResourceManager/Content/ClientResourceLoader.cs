using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientResourceManager.Util;

namespace ClientResourceManager.Content
{
    public class ClientResourceLoader : IClientResourceLoader
    {
        protected IFileSystem FileSystem { get; private set; }

        public ClientResourceLoader(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        public ClientResourceContent Load(IEnumerable<ClientResource> resources)
        {
            if (resources == null)
                return null;

            resources = resources.Where(x => x != null).ToArray();

            int resourceCount = resources.Count();

            if (resourceCount == 0)
                return new EmptyClientResourceContent();

            var contentItems = GetContentItems(resources).ToArray();

            if (contentItems.Count() == 1)
                return contentItems.Single();

            return new AggregateClientResourceContent(contentItems);
        }

        protected virtual IEnumerable<ClientResourceContent> GetContentItems(IEnumerable<ClientResource> resources)
        {
            return LoadEmbeddedResources(resources).Union(LoadLocalFiles(resources));
        }

        protected virtual IEnumerable<ClientResourceContent> LoadEmbeddedResources(IEnumerable<ClientResource> resources)
        {
            var embeddedResources =
                resources.OfType<EmbeddedClientResource>()
                    .Select(x => new EmbeddedResourceStream(x.Assembly, x.ResourceName, x.ContentType));
            return embeddedResources;
        }

        protected virtual IEnumerable<ClientResourceContent> LoadLocalFiles(IEnumerable<ClientResource> resources)
        {
            var localResources =
                resources
                    .Where(x => x.Url.IsLocalUrl())
                    .Except(resources.OfType<EmbeddedClientResource>())
                    .Select(x => new LocalFileStream(x.Url, x.ContentType));

            return localResources;
        }


        public class EmptyClientResourceContent : ClientResourceContent
        {
            public override string ContentType
            {
                get { return KnownMimeTypes.Content; }
            }

            public override bool IsValid
            {
                get { return false; }
            }

            public override void Write(Stream output)
            {
            }
        }
    }
}