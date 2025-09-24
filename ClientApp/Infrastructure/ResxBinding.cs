using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // A custom binding class to simplify binding to localized resources.
    public sealed class ResxBinding : Binding
    {
        // A custom binding helper for live language switching from .resx files.
        public ResxBinding(string path) : base($"[{path}]")
        {
            Source = TranslationSource.Instance;
        }
    }
}