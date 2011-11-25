using System;
using System.Configuration;
using System.Reflection;

namespace ClientResourceManager.Configuration
{
    public enum HandlerMode
    {
        Disabled = 0,
        HttpHandler,
        Route
    }

    public class Settings : ConfigurationSection
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

        public static Settings Current
        {
            get { return _current = _current ?? new Settings(); }
            internal set { _current = value; }
        }
        private static Settings _current;

        private const string ShowWebResourceNameKey = "showWebResourceName";
        [ConfigurationProperty(ShowWebResourceNameKey, DefaultValue = "true", IsRequired = false)]
        public bool ShowWebResourceName
        {
            get
            {
                bool result;
                bool.TryParse(this[ShowWebResourceNameKey].ToString(), out result);
                return result;
            }
            set { this[ShowWebResourceNameKey] = value; }
        }

        private const string MinificationKey = "minification";
        [ConfigurationProperty(MinificationKey, DefaultValue = "false", IsRequired = false)]
        public bool Minification
        {
            get
            {
                bool result;
                bool.TryParse(this[MinificationKey].ToString(), out result);
                return result;
            }
            set { this[MinificationKey] = value; }
        }

        private const string StreamCopyBufferSizeKey = "streamCopyBufferSize";
        [ConfigurationProperty(StreamCopyBufferSizeKey, DefaultValue = "4096", IsRequired = false)]
        public int StreamCopyBufferSize
        {
            get
            {
                int result;
                int.TryParse(this[StreamCopyBufferSizeKey].ToString(), out result);
                return result;
            }
            set { this[StreamCopyBufferSizeKey] = value; }
        }


        private const string HandlerModeKey = "handlerMode";
        [ConfigurationProperty(HandlerModeKey, DefaultValue = "Route", IsRequired = false)]
        public HandlerMode HandlerMode
        {
            get
            {
                HandlerMode mode;
                Enum.TryParse(this[HandlerModeKey].ToString(), true, out mode);

                if(mode != HandlerMode.Disabled && HandlerUrl.IsNullOrWhiteSpace())
                    throw new ConfigurationErrorsException(string.Format("'{0}' is enabled, but '{1}' setting is empty", HandlerModeKey, HandlerUrlKey));

                return mode;
            }
            set { this[HandlerModeKey] = value; }
        }

        private const string HandlerUrlKey = "handlerUrl";
        [ConfigurationProperty(HandlerUrlKey, DefaultValue = "ClientResources/{*resourceId}", IsRequired = false)]
        public string HandlerUrl
        {
            get
            {
                var url = this[HandlerUrlKey].ToString();
                return url;
            }
            set { this[HandlerUrlKey] = value; }
        }
    }
}
