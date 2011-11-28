using System.IO;
using System.Text;
using System.Web;

namespace ClientResourceManager.Filters
{
    internal class ClientResourcesPartialViewResponseFilter : ClientResourcesResponseFilter
    {
        public ClientResourcesPartialViewResponseFilter(Stream output, HttpContextBase context, ClientResourceManagerBuilder builder) 
            : base(output, context, builder)
        {
        }

        protected override void ModifyRequest(StringBuilder requestContents)
        {
            requestContents.AppendLine();

            using (var writer = new StringWriter(requestContents))
            {
                ClientResources.Render(writer, x => x.Level < Level.Global);
                ClientResources.RenderScriptStatements(writer, false);
            }
        }
    }
}