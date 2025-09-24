using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientApp.UI.ViewModels
{
    // A base class for ViewModels that implements the INotifyPropertyChanged interface.
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Occurs when a property value changes, notifying the UI.
        public event PropertyChangedEventHandler PropertyChanged;

        // Sets a property's backing field and raises the PropertyChanged event if the value has changed.
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Raises the PropertyChanged event to update the UI.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}