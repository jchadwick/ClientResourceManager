using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ClientResourceManager.Plumbing
{
    internal class ClientResourcesResponseFilter : MemoryStream
    {
        protected Encoding ContentEncoding
        {
            get { return Context.Response.ContentEncoding; }
        }

        internal HttpContextBase Context { get; set; }
        
        internal Stream OutputStream { get; set; }
        
        internal ClientResourceRegistryBuilder ResourceRegistry { get; set; }


        public ClientResourcesResponseFilter(Stream output, HttpContextBase context)
        {
            OutputStream = output;
            Context = context;
            ResourceRegistry = context.ClientResources();
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] newBuffer = buffer;
            int newOffset = offset;
            int newCount = count;

            if (ResourceRegistry != null && ResourceRegistry.Resources.Any())
            {
                var builder = new StringBuilder(ContentEncoding.GetString(buffer));

                InjectHeadResources(builder);
                InjectBodyResources(builder);

                newBuffer = ContentEncoding.GetBytes(builder.ToString());
                newOffset = 0;
                newCount = newBuffer.Length;
            }

            OutputStream.Write(newBuffer, newOffset, newCount);
        }

        private void InjectHeadResources(StringBuilder buffer)
        {
            var html = ResourceRegistry.RenderHead();
            buffer.Replace("</head>", html + "</head>");
        }

        private void InjectBodyResources(StringBuilder buffer)
        {
            var html = ResourceRegistry.Render();
            buffer.Replace("</body>", html + "</body>");
        }
    }
}