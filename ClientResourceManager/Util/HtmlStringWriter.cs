using System.IO;
using System.Web;

namespace ClientResourceManager.Util
{
    public class HtmlStringWriter : StringWriter, IHtmlString
    {
        public string ToHtmlString()
        {
            return GetStringBuilder().ToString();
        }
    }
}
