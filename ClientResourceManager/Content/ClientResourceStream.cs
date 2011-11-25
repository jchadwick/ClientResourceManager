using System;
using System.Diagnostics;
using System.IO;

namespace ClientResourceManager.Content
{
    public class ClientResourceStream : ClientResourceContent
    {
        public override string ContentType
        {
            get { return _contentType; }
        }
        private readonly string _contentType;

        public override bool IsValid
        {
            get
            {
                try
                {
                    return Stream != null && Stream.Value != null && Stream.Value.CanRead;
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning("Error loading client resource stream: " + ex.ToString());
                }

                return false;
            }
        }

        protected virtual Lazy<Stream> Stream { get; private set; }


        protected ClientResourceStream(string contentType)
        {
            _contentType = contentType;
        }

        public ClientResourceStream(Lazy<Stream> stream, string contentType)
            : this(contentType)
        {
            Stream = stream;
        }


        public override void Write(Stream output)
        {
            using(var inputStream = Stream.Value)
                CopyStream(inputStream, output);
        }
    }
}