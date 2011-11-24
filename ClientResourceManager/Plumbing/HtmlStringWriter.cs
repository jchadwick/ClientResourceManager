using System.IO;
using System.Web;

namespace ClientResourceManager.Plumbing
{
    public class HtmlStringWriter : StringWriter, IHtmlString
    {
        public string ToHtmlString()
        {
            return GetStringBuilder().ToString();
        }
    }
}
