using System.IO;

namespace ClientResourceManager.Util
{
    public interface IFileSystem
    {
        Stream Open(string filename, FileMode mode);
        FileInfo GetFileInfo(string relativePath);
    }
}
