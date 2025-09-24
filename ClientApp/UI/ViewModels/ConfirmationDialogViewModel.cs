using System;
using System.Windows.Input;
using ClientApp.Infrastructure;

namespace ClientApp.UI.ViewModels.Dialogs
{
    // ViewModel for a confirmation dialog window.
    public class ConfirmationDialogViewModel : ViewModelBase
    {
        public string Title { get; }
        public string Message { get; }
        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public event EventHandler<bool?> CloseRequested;

        // Initializes the ViewModel with a title and message.
        public ConfirmationDialogViewModel(string title, string message)
        {
            Title = title;
            Message = message;
            YesCommand = new RelayCommand(_ => Close(true));
            NoCommand = new RelayCommand(_ => Close(false));
        }

        // Invokes the CloseRequested event to close the dialog.
        private void Close(bool? result)
        {
            CloseRequested?.Invoke(this, result);
        }
    }
}