using ClientApp.DTOs;

namespace ClientApp.UI.ViewModels
{
    // A wrapper for the User model for display purposes in the UI.
    public class UserViewModel : ViewModelBase
    {
        private readonly UserDto _user;

        // Initializes the ViewModel with user data.
        public UserViewModel(UserDto user)
        {
            _user = user;
        }

        // Exposes user properties for data binding.
        public int Id => _user.Id;
        public string Username => _user.Username;
        public string FullName => _user.FullName;
        public string Role => _user.Role;
        public int TaskCount => _user.TaskCount;
        // Provides access to the underlying user data object.
        public UserDto UserData => _user;
    }
}