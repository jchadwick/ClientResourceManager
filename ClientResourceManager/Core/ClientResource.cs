using System;
using System.Diagnostics.Contracts;
using System.Web;
using ClientResourceManager.Util;

namespace ClientResourceManager
{
    public class ClientResource : IComparable<ClientResource>
    {
        private readonly string _uri;

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

        public ClientResourceKind Kind
        {
            get
            {
                return (_kind = _kind.GetValueOrDefault(GuessResourceKind())).Value;
            }
            protected set { _kind = value; }
        }
        private ClientResourceKind? _kind;

        public virtual string Key
        {
            get
            {
                if(_key == null)
                    _key = (Url.IsLocalUrl()) ? VirtualPathUtility.ToAppRelative(Url) : Url;

                return _key;
            }
        }
        private string _key;

        public Level Level { get; set; }

        public int Ordinal { get; set; }

        public string Url
        {
            get { return _url = _url ?? BuildUrl(); }
            protected set { _url = value; }
        }
        private string _url;


        protected ClientResource()
        {
        }

        public ClientResource(string uri) : this(uri, null)
        {
        }

        public ClientResource(string uri, ClientResourceKind? kind)
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
            _uri = uri;
            _kind = kind;
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
                    return KnownMimeTypes.Javascript;

                case(ClientResourceKind.Stylesheet):
                    return KnownMimeTypes.Stylesheet;

                default:
                    return KnownMimeTypes.Html;
            }
        }

        protected virtual ClientResourceKind GuessResourceKind()
        {
            return GuessResourceKind(Url);
        }

        protected virtual ClientResourceKind GuessResourceKind(string filename)
        {
            var lowerFilename = (filename ?? string.Empty).ToLowerInvariant();

            if (lowerFilename.Contains(".js"))
                return ClientResourceKind.Script;

            if (lowerFilename.Contains(".css"))
                return ClientResourceKind.Stylesheet;

            return ClientResourceKind.Content;
        }
    }
}