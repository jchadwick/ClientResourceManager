using System.Web.UI;
using DemoWebsite.Mvc.Content.EmbeddedResources;

[assembly: WebResource(Registration.EmbeddedScript, "text/javascript")]
[assembly: WebResource(Registration.JQueryValidation, "text/javascript")]

namespace DemoWebsite.Mvc.Content.EmbeddedResources
{
    public class Registration
    {
        public const string Namespace = "DemoWebsite.Mvc.Content.EmbeddedResources";

        public const string JQueryValidation = "DemoWebsite.Mvc.Scripts.jquery.validate.js";
        public const string EmbeddedScript = Namespace + ".EmbeddedScript.js";
    }
}