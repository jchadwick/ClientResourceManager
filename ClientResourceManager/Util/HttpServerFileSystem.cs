using System.IO;
using System.Web;

namespace ClientResourceManager.Util
{
    public class HttpServerFileSystem : IFileSystem
    {
        private readonly HttpServerUtilityBase _server;

        public HttpServerFileSystem(HttpServerUtility server)
            : this(new HttpServerUtilityWrapper(server))
        {
        }

        public HttpServerFileSystem(HttpServerUtilityBase server)
        {
            _server = server;
        }

        public Stream Open(string filename, FileMode mode)
        {
            var local = LocalFilename(filename);
            return File.Open(local, mode);
        }

        public FileInfo GetFileInfo(string relativePath)
        {
            var local = LocalFilename(relativePath);
            return new FileInfo(local);
        }

        protected string LocalFilename(string filename)
        {
            return _server.MapPath(filename);
        }
    }
}