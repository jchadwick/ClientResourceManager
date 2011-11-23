using System.IO;
using System.Net;
using System.Web;

namespace ClientResourceManager
{
    public class Handler : IHttpHandler
    {
        public const string ResourceIdKey = "id";

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public void ProcessRequest(HttpContextBase context)
        {
            var id = context.Request[ResourceIdKey];

            if (string.IsNullOrWhiteSpace(id))
            {
                context.Response.SetStatusCode(HttpStatusCode.BadRequest);
                return;
            }


            string filePath = context.Server.MapPath(VirtualPathUtility.ToAbsolute(id));

            if(!File.Exists(filePath))
            {
                context.Response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }


        }

    }
}
