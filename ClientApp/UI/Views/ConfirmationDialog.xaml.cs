using System.Windows;
using ClientApp.UI.ViewModels.Dialogs;

namespace ClientApp.UI.Views
{
    public partial class ConfirmationDialog : Window
    {
        // Initializes the dialog window and sets its data context.
        public ConfirmationDialog(ConfirmationDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.CloseRequested += (sender, result) =>
            {
                try
                {
                    DialogResult = result;
                }
                catch (System.InvalidOperationException)
                {
                }
                Close();
            };
        }
    }
}