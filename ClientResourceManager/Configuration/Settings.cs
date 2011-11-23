using System.Configuration;
using System.Reflection;
using System.Web;

namespace ClientResourceManager.Configuration
{
    public class Settings : ConfigurationSection
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

        public static Settings Current
        {
            get { return _current = _current ?? new Settings(); }
            internal set { _current = value; }
        }
        private static Settings _current;

        [ConfigurationProperty("showWebResourceName", DefaultValue = "false", IsRequired = false)]
        public bool ShowWebResourceName
        {
            get
            {
                bool result;
                bool.TryParse(this["showWebResourceName"].ToString(), out result);
                return result;
            }
            set { this["showWebResourceName"] = value; }
        }

        [ConfigurationProperty("handlerUrl", DefaultValue = "~/ClientResources.axd", IsRequired = false)]
        public string HandlerUrl
        {
            get
            {
                var url = this["handlerUrl"].ToString();
                return VirtualPathUtility.ToAbsolute(url);
            }
            set { this["handlerUrl"] = value; }
        }
    }
}
