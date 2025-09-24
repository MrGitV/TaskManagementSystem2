using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;
using ClientApp.UI.ViewModels.Dialogs;
using ClientApp.UI.Views;
using Task = System.Threading.Tasks.Task;

namespace ClientApp.UI.ViewModels
{
    // ViewModel for admin panel functionality.
    public class AdminViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private ObservableCollection<UserViewModel> _users;
        private UserViewModel _selectedUser;

        // Currently selected user in the UI.
        public UserViewModel SelectedUser { get => _selectedUser; set => SetProperty(ref _selectedUser, value); }
        // Collection of users displayed in admin panel.
        public ObservableCollection<UserViewModel> Users { get => _users; set => SetProperty(ref _users, value); }

        // Command to delete selected user.
        public ICommand DeleteUserCommand { get; }
        // Command to open task assignment window.
        public ICommand OpenAssignmentCommand { get; }

        // Initializes admin ViewModel with required services.
        public AdminViewModel(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
            Users = new ObservableCollection<UserViewModel>();

            DeleteUserCommand = new RelayCommand(async param => await DeleteUserAsync(param as UserViewModel), param => param is UserViewModel);
            OpenAssignmentCommand = new RelayCommand(async (param) => await OpenAssignmentWindow(param), param => param is UserViewModel);

            LoadInitialUsers();
        }

        // Loads initial user data asynchronously.
        private async void LoadInitialUsers()
        {
            await RefreshUsersAsync();
        }

        // Refreshes user list from service.
        public async Task RefreshUsersAsync()
        {
            if (_authService.CurrentRole != "Admin")
            {
                if (Users?.Any() == true)
                {
                    Application.Current.Dispatcher.Invoke(Users.Clear);
                }
                return;
            }

            try
            {
                var userModels = await _userService.GetAllUsersAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Users.Clear();
                    foreach (var user in userModels.Where(u => u.Role == UserRole.User.ToString()))
                    {
                        Users.Add(new UserViewModel(user));
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Opens task assignment dialog for selected user.
        private async Task OpenAssignmentWindow(object parameter)
        {
            if (parameter is UserViewModel userViewModel)
            {
                var taskService = App.Current.ServiceProvider.GetRequiredService<ITaskService>();
                var viewModel = new UserTaskAssignmentViewModel(userViewModel.UserData, taskService);
                var window = new UserTaskAssignmentWindow { DataContext = viewModel, Owner = Application.Current.MainWindow };
                window.ShowDialog();
                await RefreshUsersAsync();
            }
        }

        // Deletes user after confirmation dialog.
        private async Task DeleteUserAsync(UserViewModel userToDelete)
        {
            if (userToDelete == null) return;
            var message = string.Format(TranslationSource.Instance["ConfirmDeleteUserMessage"], userToDelete.Username);
            var caption = TranslationSource.Instance["ConfirmDeletionCaption"];
            var dialogViewModel = new ConfirmationDialogViewModel(caption, message);
            var dialog = new ConfirmationDialog(dialogViewModel) { Owner = Application.Current.MainWindow };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    if (await _userService.DeleteUserAsync(userToDelete.Id))
                    {
                        Users.Remove(userToDelete);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}