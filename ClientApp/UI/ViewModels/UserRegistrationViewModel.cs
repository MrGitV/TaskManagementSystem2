using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;

namespace ClientApp.UI.ViewModels
{
    // ViewModel for the user registration window.
    public class UserRegistrationViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;
        private string _fullName;
        private string _statusMessage;
        private Brush _statusMessageColor;

        // The new user's username.
        public string Username { get => _username; set => SetProperty(ref _username, value); }
        // The new user's password.
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        // The new user's full name.
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        // A message to show the status of the registration process.
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
        // The color of the status message (e.g., green for success, red for error).
        public Brush StatusMessageColor { get => _statusMessageColor; set => SetProperty(ref _statusMessageColor, value); }

        // The command to execute the registration.
        public ICommand RegisterCommand { get; }

        // ViewModel constructor.
        public UserRegistrationViewModel(IUserService userService)
        {
            _userService = userService;
            RegisterCommand = new RelayCommand(async _ => await RegisterUserAsync(), CanRegister);
        }

        // Determines if the registration command can be executed.
        private bool CanRegister(object _)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(FullName);
        }

        // Handles the user registration logic.
        private async Task RegisterUserAsync()
        {
            try
            {
                var newUser = new CreateUserRequest
                {
                    Username = Username,
                    Password = Password,
                    FullName = FullName
                };
                var response = await _userService.CreateUserAsync(newUser);
                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = TranslationSource.Instance["RegistrationSuccess"];
                    StatusMessageColor = Brushes.Green;
                    Username = string.Empty;
                    Password = string.Empty;
                    FullName = string.Empty;
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    StatusMessage = TranslationSource.Instance["UsernameExistsError"];
                    StatusMessageColor = Brushes.Red;
                }
                else
                {
                    StatusMessage = TranslationSource.Instance["RegistrationError"];
                    StatusMessageColor = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"{TranslationSource.Instance["RegistrationError"]}: {ex.Message}";
                StatusMessageColor = Brushes.Red;
            }
        }
    }
}