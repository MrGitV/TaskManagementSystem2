using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientApp.Infrastructure;
using ClientApp.Services;

namespace ClientApp.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;   // injected authentication service
        private string _username;                     // backing field for Username
        private string _password;                     // backing field for Password
        private string _errorMessage;                 // backing field for ErrorMessage

        // user’s login name
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        // user’s password
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // message shown on login failure
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }         // command bound to login button
        public event Action LoginSuccess;             // event raised on successful login

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService
                ?? throw new ArgumentNullException(nameof(authService));                     // ensure authService is provided
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin()); // set up login command
        }

        // allow login only if fields are filled
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password);
        }

        // Perform the async login workflow
        private async Task LoginAsync()
        {
            try
            {
                ErrorMessage = string.Empty;                                     // clear previous errors

                var success = await _authService.LoginAsync(Username, Password); // call auth service
                if (success)
                {
                    LoginSuccess?.Invoke();                                      // notify success
                }
                else
                {
                    ErrorMessage = TranslationSource.Instance["InvalidCredentialsError"]; // invalid credentials
                    OnPropertyChanged(nameof(ErrorMessage));                              // update UI
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{TranslationSource.Instance["LoginException"]}: {ex.Message}"; // show exception
                OnPropertyChanged(nameof(ErrorMessage));                                       // update UI
            }
        }
    }
}
