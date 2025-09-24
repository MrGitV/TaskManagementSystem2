using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ClientApp.Infrastructure;
using ClientApp.UI.ViewModels;

namespace ClientApp.UI.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        // Initializes the window and subscribes to the LoginSuccess event.
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.LoginSuccess += () =>
            {
                DialogResult = true;
                Close();
            };
        }

        // Updates the ViewModel's password property when the password box content changes.
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.Password = (sender as PasswordBox)?.Password;
            }
        }

        // Handles the exit button click to shut down the application.
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Changes the application's language based on the combo box selection.
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selectedItem && selectedItem.Tag is string culture)
            {
                var newCulture = new CultureInfo(culture);
                TranslationSource.Instance.CurrentCulture = newCulture;
            }
        }
    }
}