using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Windows;
using ClientApp.Services;
using ClientApp.Services.Http;
using ClientApp.UI.ViewModels;
using ClientApp.UI.Views;

namespace ClientApp
{
    // Main application class, handles startup and service configuration.
    public partial class App : Application
    {
        // Provides a static reference to the current App instance.
        public new static App Current => (App)Application.Current;
        // The service provider for dependency injection.
        public IServiceProvider ServiceProvider { get; private set; }

        // Handles the application startup event.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            ShowLogin();
        }

        // Configures services for dependency injection.
        private void ConfigureServices(ServiceCollection services)
        {
            // Base URL for the backend API.
            string apiBaseUrl = "https://localhost:14502";

            // Register a message handler to add auth tokens to requests.
            services.AddTransient<AuthHeaderHandler>();

            // Register IAuthService with a dedicated HttpClient.
            services.AddSingleton<IAuthService, AuthService>(provider =>
            {
                var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient("AuthClient");
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                return new AuthService(httpClient);
            });

            // Register other API services with HttpClients that use the AuthHeaderHandler.
            services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddHttpClient<ITaskService, TaskService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddHttpClient<IProjectService, ProjectService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<AuthHeaderHandler>();


            // Register ViewModels as transient services.
            services.AddTransient<LoginViewModel>();
            services.AddTransient<UserRegistrationViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<CalendarViewModel>();
            services.AddTransient<AdminViewModel>();
            services.AddTransient<UserTaskAssignmentViewModel>();
            services.AddTransient<UserTaskViewViewModel>();

            // Register Windows as transient services.
            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<UserRegistrationWindow>();
            services.AddTransient<TaskDetailsWindow>();
            services.AddTransient<UserTaskAssignmentWindow>();
            services.AddTransient<UserTaskViewWindow>();
        }

        // Manages the login flow and transitions to the main window.
        public void ShowLogin()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainWindow?.Close();
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            if (loginWindow.ShowDialog() == true)
            {
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                MainWindow = mainWindow;
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}