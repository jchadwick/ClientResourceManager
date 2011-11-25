using System.ComponentModel;

namespace ClientResourceManager.Util
{
    /// <summary>
    /// Hides all the core object methods from Intellisense
    /// so that the useful methods are more discoverable
    /// </summary>
    public interface IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}
