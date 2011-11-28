using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClientResourceManager.Content
{
    public class AggregateClientResourceContent : ClientResourceContent
    {
        public List<ClientResourceContent> Contents { get; private set; }

        public override DateTime? LastModified
        {
            get { return base.LastModified ?? Contents.Max(x => x.LastModified); }
            set { base.LastModified = value; }
        }

        public override string ContentType
        {
            get { return _contentType = _contentType ?? Contents.Max(x => x.ContentType); }
        }
        private string _contentType;

        public override bool IsValid
        {
            get { return Contents.All(x => x.IsValid); }
        }


        public AggregateClientResourceContent() : this(null)
        {
        }

        public AggregateClientResourceContent(IEnumerable<ClientResourceContent> contents)
        {
            Contents = new List<ClientResourceContent>(contents ?? Enumerable.Empty<ClientResourceContent>());
        }


        public override void Write(Stream output)
        {
            var contents = Contents.Where(x => x != null).ToArray();
            foreach (var content in contents)
            {
                content.Write(output);
            }
        }
    }
}