using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;
using ClientResourceManager.Configuration;
using ClientResourceManager.Filters;

namespace ClientResourceManager
{
    public class Module : IHttpModule
    {
        static Module()
        {
            Settings.Current = ConfigurationManager.GetSection("clientResourceManager") as Settings ?? new Settings();

            if (Settings.Current.HandlerMode == HandlerMode.Route)
                RouteTable.Routes.Insert(0, new Route(Settings.Current.HandlerUrl, new ClientResourceRouteHandler()));
        }

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
            if (RequestIsWebResource(context))
                return;

            var builder = context.ClientResources();

            if(context.IsAjaxRequest())
                context.Response.Filter = new ClientResourcesAjaxResponseFilter(context.Response.Filter, context, builder);
            else
                context.Response.Filter = new ClientResourcesResponseFilter(context.Response.Filter, context, builder);

            context.Trace.Write("ClientResourceManager", "Injected client resources into response");
        }

        private static bool RequestIsWebResource(HttpContextBase context)
        {
            return context.Request.Url.ToString().Contains("WebResource.axd");
        }
    }
}
