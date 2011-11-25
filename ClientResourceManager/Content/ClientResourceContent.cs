using System;
using System.IO;
using System.Text;
using System.Web;
using ClientResourceManager.Configuration;

namespace ClientResourceManager.Content
{
    public abstract class ClientResourceContent
    {
        public virtual HttpCacheability Cacheability { get; set; }
        
        public abstract string ContentType { get; }
        
        public virtual Encoding Encoding { get; set; }
        
        public virtual DateTime? LastModified { get; set; }

        public virtual DateTime? ExpirationDate
        {
            get { return _expirationDate ?? DateTime.UtcNow.AddYears(1); }
            set { _expirationDate = value; }
        }
        private DateTime? _expirationDate;

        public abstract bool IsValid { get; }


        protected ClientResourceContent()
        {
            Cacheability = HttpCacheability.Public;
            Encoding = Encoding.Default;
        }


        public abstract void Write(Stream output);

        protected static void CopyStream(Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");
            if (outputStream == null)
                throw new ArgumentNullException("outputStream");

            byte[] buffer = new byte[Settings.Current.StreamCopyBufferSize];
            int bytesRead;
            do
            {
                bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                outputStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }
    }
}