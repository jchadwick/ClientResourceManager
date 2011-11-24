using System.Web.UI;
using DemoWebsite.Mvc.Content.EmbeddedResources;

[assembly: WebResource(Registration.EmbeddedScript, "text/javascript")]

namespace DemoWebsite.Mvc.Content.EmbeddedResources
{
    public class Registration
    {
        public const string EmbeddedScript = "DemoWebsite.Mvc.Content.EmbeddedResources.EmbeddedScript.js";
    }
}