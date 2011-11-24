using System.Web.Mvc;

namespace ClientResourceManager.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static ClientResourceManagerBuilder ClientResources(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.ClientResources();
        }
    }
}
