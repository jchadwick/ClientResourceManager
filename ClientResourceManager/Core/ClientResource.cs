using System;
using System.Diagnostics.Contracts;
using System.Web;

namespace ClientResourceManager
{
    public class ClientResource : IComparable<ClientResource>
    {
        private readonly string _uri;

        public int Ordinal { get; set; }

        public string ContentType
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

        public ClientResourceKind Kind { get; set; }

        public string Url
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
            if (ReferenceEquals(other, null) || ReferenceEquals(other.Level, null)) return 1;

            if (Level == null) return -1;

            return Level.CompareTo(other.Level);
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