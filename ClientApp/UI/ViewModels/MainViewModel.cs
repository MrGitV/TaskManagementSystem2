using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;
using ClientApp.Infrastructure;
using ClientApp.Services;
using ClientApp.UI.Views;

namespace ClientApp.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;
        private string _statusMessage;

        public CalendarViewModel CalendarViewModel { get; }
        public AdminViewModel AdminViewModel { get; }

        // Check if current user has Admin role using available properties
        public bool IsAdmin => _authService.CurrentRole == "Admin";

        // Control admin panel visibility based on user role
        public Visibility AdminPanelVisibility => IsAdmin ? Visibility.Visible : Visibility.Collapsed;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand LogoutCommand { get; }
        public ICommand OpenRegistrationCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel(IAuthService authService, IServiceProvider serviceProvider,
                             CalendarViewModel calendarVM, AdminViewModel adminVM)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;

            CalendarViewModel = calendarVM;
            AdminViewModel = adminVM;

            // Subscribe to role changes if available, otherwise use direct check
            UpdateStatusMessage();

            LogoutCommand = new RelayCommand(_ => Logout());
            OpenRegistrationCommand = new RelayCommand(_ => OpenRegistrationWindow(), _ => IsAdmin);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        // Update status bar with available user information
        private void UpdateStatusMessage()
        {
            StatusMessage = _authService.CurrentUserId != null
                ? $"{_authService.CurrentRole} User"
                : "";
        }

        // Open user registration window (admin only)
        private async void OpenRegistrationWindow()
        {
            var registrationWindow = _serviceProvider.GetRequiredService<UserRegistrationWindow>();
            registrationWindow.Owner = Application.Current.MainWindow;
            registrationWindow.ShowDialog();

            // Refresh data after registration if user is admin
            if (IsAdmin)
            {
                await AdminViewModel.RefreshUsersAsync();
                CalendarViewModel.RefreshCalendar();
            }
        }

        // Logout and return to login screen
        private void Logout()
        {
            _authService.Logout();
            App.Current.ShowLogin();
        }
    }
}