using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web;
using System.Web.Handlers;
using ClientResourceManager.Configuration;

namespace ClientResourceManager
{
    public static class AssemblyExtensions
    {

        // As seen here: http://www.eworldui.net/blog/post/2008/05/13/ASPNET-MVC-Extracting-Web-Resources.aspx
        public static string GetWebResourceUrl(this Assembly targetAssembly, string resourceName)
        {
            Contract.Requires(resourceName.HasValue());

            if (_getWebResourceUrlMethod == null)
            {
                lock (GetWebResourceUrlLock)
                {
                    var method = _getWebResourceUrlMethod;
                    if (method == null)
                    {
                        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
                        method = typeof(AssemblyResourceLoader).GetMethod("GetWebResourceUrlInternal", bindingFlags);
                    }
                    _getWebResourceUrlMethod = method;
                }
            }

            var resourceUrl = (string) _getWebResourceUrlMethod.Invoke(null, new object[] { targetAssembly, resourceName, false, false, null });

            // Add the Resource Name to the URL for easier debugging
            if (Settings.Current.ShowWebResourceName)
            {
                var encodedName = HttpUtility.UrlEncode(resourceName);
                var queryParameter = string.Format("ResourceName={0}&", encodedName);
                var queryStringStart = resourceUrl.IndexOf('?');
                resourceUrl = resourceUrl.Insert(queryStringStart + 1, queryParameter);
            }

            return resourceUrl;
        }
        private static readonly object GetWebResourceUrlLock = new object();
        private static MethodInfo _getWebResourceUrlMethod;

    }
}
