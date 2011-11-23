using System;
using System.Web.Mvc;

namespace ClientResourceManager.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string GetWebResourceUrl<T>(this UrlHelper helper, string resourceName)
        {
            var url = typeof(T).Assembly.GetWebResourceUrl(resourceName);
            return url;
        }
        public static string GetWebResourceUrl(this UrlHelper helper, Type type, string resourceName)
        {
            var url = type.Assembly.GetWebResourceUrl(resourceName);
            return url;
        }

    }
}
