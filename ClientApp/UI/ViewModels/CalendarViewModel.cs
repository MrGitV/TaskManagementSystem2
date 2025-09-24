using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;
using ClientApp.UI.Views;
namespace ClientApp.UI.ViewModels
{
    // ViewModel for the calendar view.
    public class CalendarViewModel : ViewModelBase, IDisposable
    {
        private readonly IAuthService _authService;
        private readonly ITaskService _taskService;
        private DateTime _currentDate;
        private ObservableCollection<DayCellViewModel> _days;
        private string _monthYearHeader;

        // The currently displayed date.
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                if (SetProperty(ref _currentDate, value))
                {
                    UpdateMonthYearHeader();
                    LoadCalendar();
                }
            }
        }

        // Collection of days to display in the calendar grid.
        public ObservableCollection<DayCellViewModel> Days
        {
            get => _days;
            set => SetProperty(ref _days, value);
        }

        // Header with the month name and year.
        public string MonthYearHeader
        {
            get => _monthYearHeader;
            set => SetProperty(ref _monthYearHeader, value);
        }

        // Command to navigate to the previous month.
        public ICommand PreviousMonthCommand { get; }
        // Command to navigate to the next month.
        public ICommand NextMonthCommand { get; }
        // Command executed when a day is selected.
        public ICommand DaySelectedCommand { get; }

        // ViewModel constructor.
        public CalendarViewModel(IAuthService authService, ITaskService taskService)
        {
            _authService = authService;
            _taskService = taskService;
            CurrentDate = DateTime.Today;

            TranslationSource.Instance.PropertyChanged += TranslationSource_PropertyChanged;

            PreviousMonthCommand = new RelayCommand(_ => CurrentDate = CurrentDate.AddMonths(-1));
            NextMonthCommand = new RelayCommand(_ => CurrentDate = CurrentDate.AddMonths(1));
            DaySelectedCommand = new RelayCommand(OnDaySelected, CanDayBeSelected);

            LoadCalendar();
        }

        // Language change handler.
        private void TranslationSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateMonthYearHeader();
        }

        // Updates the header with the month and year.
        private void UpdateMonthYearHeader()
        {
            string monthKey = $"Month{CurrentDate.Month}";
            MonthYearHeader = $"{TranslationSource.Instance[monthKey]} {CurrentDate.Year}";
        }

        // Releases resources.
        public void Dispose()
        {
            TranslationSource.Instance.PropertyChanged -= TranslationSource_PropertyChanged;
        }

        // Asynchronously loads the days and tasks for the current month.
        private async void LoadCalendar()
        {
            if (_authService?.CurrentUserId == null) return;
            Days = new ObservableCollection<DayCellViewModel>();
            var firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<TaskDto> tasks;
            if (_authService.CurrentRole == "Admin")
            {
                tasks = await _taskService.GetAllTasksForDateRangeAsync(firstDayOfMonth, lastDayOfMonth);
            }
            else
            {
                tasks = await _taskService.GetTasksForUserAsync(_authService.CurrentUserId.ToString(), firstDayOfMonth, lastDayOfMonth);
            }
            int daysOffset = (int)firstDayOfMonth.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (daysOffset < 0) daysOffset += 7;
            for (int i = 0; i < daysOffset; i++)
                Days.Add(new DayCellViewModel { IsEmpty = true });
            for (int day = 1; day <= lastDayOfMonth.Day; day++)
            {
                var date = new DateTime(CurrentDate.Year, CurrentDate.Month, day);
                var dayTasks = tasks.Where(t => t.DueDate.Date == date.Date).ToList();
                Days.Add(new DayCellViewModel(date, dayTasks));
            }
            int totalCells = 42;
            int trailingEmpty = totalCells - Days.Count;
            for (int i = 0; i < trailingEmpty; i++)
                Days.Add(new DayCellViewModel { IsEmpty = true });
        }

        // Property to check if the current user is an administrator.
        public bool IsAdmin => _authService.CurrentRole == "Admin";

        // Checks if a day can be selected (not an empty cell).
        private bool CanDayBeSelected(object parameter)
        {
            return parameter is DayCellViewModel dayCell && !dayCell.IsEmpty;
        }

        // Handles the selection of a day in the calendar.
        private void OnDaySelected(object parameter)
        {
            if (parameter is DayCellViewModel dayCell && !dayCell.IsEmpty)
            {
                var tasks = dayCell.Tasks.Select(t => t.TaskData).ToList();
                if (IsAdmin)
                {
                    var adminViewModel = new TaskDetailsViewModel(dayCell.Date, tasks, _taskService, _authService);
                    var adminWindow = new TaskDetailsWindow { DataContext = adminViewModel, Owner = Application.Current.MainWindow };
                    adminWindow.ShowDialog();
                }
                else
                {
                    var userViewModel = new UserTaskViewViewModel(tasks, _taskService, _authService);
                    var userWindow = new UserTaskViewWindow { DataContext = userViewModel, Owner = Application.Current.MainWindow };
                    userWindow.ShowDialog();
                }
                RefreshCalendar();
            }
        }

        // Public method to refresh the calendar from the outside.
        public void RefreshCalendar() => LoadCalendar();
    }
}