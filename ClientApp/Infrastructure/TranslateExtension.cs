using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ClientApp.Infrastructure
{
    // A markup extension to provide localized strings from resource files in XAML.
    public class TranslateExtension : MarkupExtension
    {
        // The key of the resource string to retrieve.
        public string Key { get; set; }

        // Initializes a new instance of the TranslateExtension.
        public TranslateExtension() { }

        // Initializes a new instance with a specified key.
        public TranslateExtension(string key)
        {
            Key = key;
        }

        // Provides the binding to the localized resource.
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding($"[{Key}]")
            {
                Source = TranslationSource.Instance,
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}