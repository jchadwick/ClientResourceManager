using System;
using System.Diagnostics.Contracts;
using System.Web;

namespace ClientResourceManager
{
    public class ClientResource : IComparable<ClientResource>
    {
        private readonly string _uri;

        public virtual string ContentType
        {
            get
            {
                if (_contentType == null)
                    return GetContentType(Kind);

                return _contentType;
            }
            set { _contentType = value; }
        }
        private string _contentType;

        public virtual ClientResourceKind Kind { get; set; }

        public virtual string Url
        {
            get { return _url = _url ?? BuildUrl(); }
            protected set { _url = value; }
        }
        private string _url;

        public Level Level { get; set; }


        protected ClientResource()
        {
        }

        public ClientResource(string uri)
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
            _uri = uri;
        }


        public int CompareTo(ClientResource other)
        {
            var value = (other == null) ? String.Empty : other.Url;
            return StringComparer.OrdinalIgnoreCase.Compare(Url, value);
        }

        protected virtual string BuildUrl()
        {
            if(_uri != null)
                return VirtualPathUtility.ToAbsolute(_uri);
            
            return null;
        }

        protected virtual string GetContentType(ClientResourceKind kind)
        {
            switch (kind)
            {
                case(ClientResourceKind.Script):
                    return "text/javascript";

                case(ClientResourceKind.Stylesheet):
                    return "text/stylesheet";

                default:
                    return "text/html";
            }
        }
    }
}