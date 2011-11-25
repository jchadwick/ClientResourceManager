using System;
using System.Diagnostics.Contracts;
using System.IO;
using ClientResourceManager.Util;

namespace ClientResourceManager.Content
{
    public class LocalFileStream : ClientResourceStream, IDisposable
    {
        private readonly FileInfo _fileInfo;

        public override DateTime? LastModified
        {
            get { return _fileInfo.LastWriteTimeUtc; }
            set { /* Readonly */ }
        }

        public string RelativePath { get; private set; }

        protected override Lazy<Stream> Stream
        {
            get { return _stream; }
        }
        private readonly Lazy<Stream> _stream;


        public LocalFileStream(string relativePath, string contentType)
            : this(ServiceLocator.Resolve<IFileSystem>(), relativePath, contentType)
        {
        }

        public LocalFileStream(IFileSystem fileSystem, string relativePath, string contentType) 
            : base(contentType)
        {
            Contract.Requires(fileSystem != null);

            RelativePath = relativePath;
            _fileInfo = fileSystem.GetFileInfo(relativePath);
            _stream = new Lazy<Stream>(() => fileSystem.Open(relativePath, FileMode.Open));
        }

        public void Dispose()
        {
            if(Stream.IsValueCreated && Stream.Value != null)
                Stream.Value.Dispose();
        }
    }
}