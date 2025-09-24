using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ClientApp.Infrastructure;
using ClientApp.UI.ViewModels;
namespace ClientApp.UI.Views
{
    public partial class TaskDetailsWindow : Window
    {
        // Initializes the component.
        public TaskDetailsWindow()
        {
            InitializeComponent();
        }

        //Window closing event handler to refresh the calendar.
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.DataContext is MainViewModel mainVM)
            {
                mainVM.CalendarViewModel.RefreshCalendar();
            }
        }

        // Handles language change from the dropdown list.
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selectedItem && selectedItem.Tag is string culture)
            {
                TranslationSource.Instance.CurrentCulture = new CultureInfo(culture);
            }
        }
    }
}