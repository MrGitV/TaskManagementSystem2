using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ClientApp.Infrastructure;
using ClientApp.UI.ViewModels;

namespace ClientApp.UI.Views
{
    public partial class UserRegistrationWindow : Window
    {
        private readonly UserRegistrationViewModel _viewModel;

        // Initializes the window and sets up its data context.
        public UserRegistrationWindow(UserRegistrationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        // Clears the password box when the ViewModel's password property is reset.
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Password) && string.IsNullOrEmpty(_viewModel.Password))
            {
                PasswordBox.Clear();
            }
        }

        // Updates the ViewModel's password property when the password box content changes.
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = (sender as PasswordBox)?.Password;
        }

        // Sets focus back to the owner window upon closing.
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Owner?.Focus();
        }

        // Changes the application's language based on the combo box selection.
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selectedItem && selectedItem.Tag is string culture)
            {
                TranslationSource.Instance.CurrentCulture = new CultureInfo(culture);
            }
        }

        // Unsubscribes from the PropertyChanged event to prevent memory leaks.
        protected override void OnClosed(EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            base.OnClosed(e);
        }
    }
}