using System;
using System.Web;
using ClientResourceManager.Plumbing;

namespace ClientResourceManager
{
    public class Module : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostReleaseRequestState += OnPostReleaseRequestState;
        }

        private static void OnPostReleaseRequestState(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            
            if (httpApplication == null)
                return;

            var httpContext = new HttpContextWrapper(httpApplication.Context);

            try
            {
                PostReleaseRequestState(httpContext);
            }
            catch (Exception exception)
            {
                httpContext.Trace.Warn("ClientResourceManager", "Error injecting client resources into response: " + exception);
            }
        }

        private static void PostReleaseRequestState(HttpContextBase context)
        {
            if (!context.IsAjax())
                context.Response.Filter = new ClientResourcesResponseFilter(context.Response.Filter, context);

            context.Trace.Write("ClientResourceManager", "Injected client resources into response");
        }
    }
}
