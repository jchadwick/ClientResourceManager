using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ClientResourceManager.Filters
{
    internal class ClientResourcesResponseFilter : MemoryStream
    {
        protected Encoding ContentEncoding
        {
            get { return Context.Response.ContentEncoding; }
        }

        protected internal HttpContextBase Context { get; private set; }

        protected internal Stream OutputStream { get; private set; }

        protected internal ClientResourceManagerBuilder ClientResources { get; private set; }


        public ClientResourcesResponseFilter(Stream output, HttpContextBase context, ClientResourceManagerBuilder builder)
        {
            OutputStream = output;
            Context = context;
            ClientResources = builder;
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] newBuffer = buffer;
            int newOffset = offset;
            int newCount = count;

            if (ClientResources != null && ClientResources.Resources.Any())
            {
                var builder = new StringBuilder(ContentEncoding.GetString(buffer));

                ModifyRequest(builder);

                newBuffer = ContentEncoding.GetBytes(builder.ToString());
                newOffset = 0;
                newCount = newBuffer.Length;
            }

            OutputStream.Write(newBuffer, newOffset, newCount);
        }

        protected virtual void ModifyRequest(StringBuilder requestContents)
        {
            InjectHeadResources(requestContents);
            InjectBodyResources(requestContents);
        }

        private void InjectHeadResources(StringBuilder buffer)
        {
            var html = ClientResources.RenderHead();
            buffer.Replace("</head>", html + "</head>");
        }

        private void InjectBodyResources(StringBuilder buffer)
        {
            var html = ClientResources.Render();
            buffer.Replace("</body>", html + "</body>");
        }
    }
}