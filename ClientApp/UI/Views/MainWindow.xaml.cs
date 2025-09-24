using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ClientApp.Infrastructure;
using ClientApp.UI.ViewModels;

namespace ClientApp.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        // Initializes a new instance of the MainWindow and sets its DataContext.
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Closed += MainWindow_Closed;
        }

        // Disposes of resources when the window is closed to prevent memory leaks.
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (_viewModel?.CalendarViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        // Refreshes the admin view when its tab is selected.
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl &&
                tabControl.SelectedIndex == 1 && // Admin tab
                _viewModel.IsAdmin)
            {
                await _viewModel.AdminViewModel.RefreshUsersAsync();
            }
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