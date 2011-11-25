using System.Web;
using ClientResourceManager;
using ClientResourceManager.Content;
using ClientResourceManager.Core;
using ClientResourceManager.Util;

[assembly: PreApplicationStartMethod(typeof(AppStart), "Initialize")]

namespace ClientResourceManager
{
    public static class AppStart
    {
        public static void Initialize()
        {
            ServiceLocator.Register<IClientResourceRepository>(new ClientResourceRepositoryFactory().Create);
            ServiceLocator.Register<IClientResourceLoader>(() => new ClientResourceLoader(ServiceLocator.Resolve<IFileSystem>()));
            ServiceLocator.Register<IFileSystem>(() => new HttpServerFileSystem(HttpContext.Current.Server));
        }
    }
}
