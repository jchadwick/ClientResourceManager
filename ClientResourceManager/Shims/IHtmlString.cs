// CodeContracts.cs: Lightweight < .NET 4 shim for Code Contracts

#if(!DOTNET4)

namespace System.Web
{
    public interface IHtmlString
    {
        string ToHtmlString();
    }

    public class HtmlString : IHtmlString
    {
        private readonly string _value;

        public HtmlString(string value)
        {
            _value = value;
        }

        public string ToHtmlString()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}

#endif