using System.Web.Mvc;

namespace ClientResourceManager.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static ClientResourceRegistryBuilder ClientResources(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.ClientResources();
        }
    }
}
