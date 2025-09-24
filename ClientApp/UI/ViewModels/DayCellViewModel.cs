using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClientApp.DTOs;

namespace ClientApp.UI.ViewModels
{
    // Represents a single day cell in the calendar view.
    public class DayCellViewModel : ViewModelBase
    {
        // The date this cell represents.
        public DateTime Date { get; }
        // Indicates if cell is a placeholder for empty days in the calendar grid.
        public bool IsEmpty { get; set; }
        // Collection of tasks scheduled for this day.
        public ObservableCollection<TaskCellViewModel> Tasks { get; }

        // The day number to display in the cell.
        public string DayNumber => IsEmpty ? "" : Date.Day.ToString();
        // Checks if the date falls on a weekend.
        public bool IsWeekend => !IsEmpty && (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday);
        // Checks if the date is today.
        public bool IsToday => !IsEmpty && Date.Date == DateTime.Today;
        // Tooltip showing comma-separated task titles.
        public string TaskTitlesTooltip => IsEmpty ? null : string.Join(", ", Tasks.Select(t => t.Title));

        // Determines if an icon for "all tasks completed" should be shown.
        public bool ShowCompletedIcon => !IsEmpty && Tasks.Any() && Tasks.All(t => t.Status == TaskStatus.Completed);
        // Returns tasks to display, hiding them if the "all completed" icon is shown.
        public ObservableCollection<TaskCellViewModel> DisplayTasks => ShowCompletedIcon ? new ObservableCollection<TaskCellViewModel>() : Tasks;

        // Initializes an empty day cell.
        public DayCellViewModel()
        {
            IsEmpty = true;
            Tasks = new ObservableCollection<TaskCellViewModel>();
        }

        // Initializes a day cell for a specific date with its list of tasks.
        public DayCellViewModel(DateTime date, List<TaskDto> tasks)
        {
            Date = date;
            IsEmpty = false;
            var sortedTasks = tasks.OrderBy(t => t.DueDate).Select(t => new TaskCellViewModel(t));
            Tasks = new ObservableCollection<TaskCellViewModel>(sortedTasks);
        }
    }
}