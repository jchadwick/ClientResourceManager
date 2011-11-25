using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using ClientResourceManager.Configuration;
using ClientResourceManager.Content;
using ClientResourceManager.Content.Processors;
using ClientResourceManager.Core;
using ClientResourceManager.Util;

namespace ClientResourceManager
{
    public class ClientResourceRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Handler { ResourceId = requestContext.RouteData.Values["resourceId"] as string };
        }
    }

    public class Handler : IHttpHandler
    {
        private readonly IClientResourceRepository _repository;
        private readonly IClientResourceLoader _loader;

        public bool IsReusable
        {
            get { return false; }
        }

        public string ResourceId { get; set; }

        public static string HandlerUrlFormat
        {
            get
            {
                if(_handlerUrlFormat == null)
                {
                    var handlerUrl = Settings.Current.HandlerUrl;
                    var mode = Settings.Current.HandlerMode;

                    // The HandlerUrl setting can be a route or a relative HTTP handler url.
                    // Figure out which one it is and create at format string where {0} 
                    // is the resource ID.

                    // If it's a route, replace {*resourceId} with the 
                    // resource ID placeholder ({0})
                    if (mode == HandlerMode.Route)
                        _handlerUrlFormat = "~/" + handlerUrl.Replace("{*resourceId}", "{0}");

                    // Otherwise it's a standard handler, so use the querystring format
                    else if (mode == HandlerMode.HttpHandler)
                        _handlerUrlFormat = string.Format("{0}?{1}={{0}}", handlerUrl, "id");

                    else
                        _handlerUrlFormat = "{0}";
                }

                return _handlerUrlFormat;
            }
        }
        private volatile static string _handlerUrlFormat;


        public Handler()
            : this(ServiceLocator.Resolve<IClientResourceRepository>(), ServiceLocator.Resolve<IClientResourceLoader>())
        {
        }

        public Handler(IClientResourceRepository repository, IClientResourceLoader loader)
        {
            _repository = repository;
            _loader = loader;
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        protected internal virtual void ProcessRequest(HttpContextBase context)
        {
            var resourceId = ResourceId ?? context.Request["id"];

            if (resourceId.IsNullOrWhiteSpace())
            {
                context.SetStatusCode(HttpStatusCode.BadRequest, "Missing resource id");
                return;
            }


            var resources = _repository.GetByKey(resourceId) ?? Enumerable.Empty<ClientResource>();

            if (resources.Any() == false)
            {
                context.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }


            var content = _loader.Load(resources);

            if (context.HasBeenModifiedSince(content.LastModified) == false)
            {
                context.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }


            Render(context, content);
        }

        protected internal virtual void Render(HttpContextBase context, ClientResourceContent resource)
        {
            context.Response.Clear();

            context.Response.Buffer = true;
            context.Response.ContentType = resource.ContentType;
            context.Response.ContentEncoding = resource.Encoding;

            ApplyCaching(context, resource);

            new Minifier().Write(resource, context.Response.OutputStream);
        }

        protected internal virtual void ApplyCaching(HttpContextBase context, ClientResourceContent resource)
        {
            context.Response.Cache.SetCacheability(resource.Cacheability);

            if (resource.ExpirationDate != null)
            {
                context.Response.Cache.SetExpires(resource.ExpirationDate.Value);
                context.Response.Cache.SetMaxAge(new TimeSpan(resource.ExpirationDate.Value.ToFileTimeUtc()));
            }

            if (resource.LastModified != null)
            {
                context.Response.Cache.SetLastModified(resource.LastModified.Value);
            }
        }

        public static string GenerateUrl(params ClientResource[] resources)
        {
            var repository = ServiceLocator.Resolve<IClientResourceRepository>();

            var key = repository.GetKey(resources);

            if (key.Contains("?"))
                key = HttpUtility.UrlEncode(key);

            return string.Format(HandlerUrlFormat, key);
        }
    }
}
