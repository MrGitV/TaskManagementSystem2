using System.ComponentModel;
using System.Globalization;
using System.Resources;
using ClientApp.Resources;

namespace ClientApp.Infrastructure
{
    // A singleton class that provides localized strings and notifies the UI of culture changes.
    public sealed class TranslationSource : INotifyPropertyChanged
    {
        private static readonly TranslationSource _instance = new TranslationSource();
        private ResourceManager _resourceManager;
        private CultureInfo _currentCulture = new CultureInfo("en");

        // Occurs when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Gets the singleton instance of the TranslationSource.
        public static TranslationSource Instance => _instance;

        // Indexer to get a localized string for a given key.
        public string this[string key] =>
            _resourceManager?.GetString(key, _currentCulture) ?? key;

        // Gets or sets the current culture for localization.
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    UpdateResourceManager();

                    System.Threading.Thread.CurrentThread.CurrentCulture = value;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                }
            }
        }

        // Private constructor for the singleton pattern.
        private TranslationSource()
        {
            UpdateResourceManager();
        }

        // Updates the ResourceManager based on the current culture.
        private void UpdateResourceManager()
        {
            switch (_currentCulture.TwoLetterISOLanguageName)
            {
                case "ru":
                    _resourceManager = ru.ResourceManager;
                    break;
                default:
                    _resourceManager = en.ResourceManager;
                    break;
            }
        }
    }
}