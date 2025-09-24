using System;
using ClientApp.DTOs;

namespace ClientApp.UI.ViewModels
{
    // ViewModel for a single task cell displayed in the calendar.
    public class TaskCellViewModel : ViewModelBase
    {
        private readonly TaskDto _task;

        // Initializes the ViewModel with task data.
        public TaskCellViewModel(TaskDto task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        // Exposes the raw task data.
        public TaskDto TaskData => _task;
        // The title of the task.
        public string Title => _task.Title;
        // The status of the task.
        public TaskStatus Status => _task.Status;

        // Determines the icon symbol based on task status and due date.
        public string IconSymbol
        {
            get
            {
                if (IsOverdue) return "−";
                switch (_task.Status)
                {
                    case TaskStatus.Completed: return "+";
                    case TaskStatus.AwaitingReview: return "!";
                    case TaskStatus.NeedsWork: return "?";
                    case TaskStatus.InProgress: return "";
                    default: return "";
                }
            }
        }

        // Determines the background color of the task cell.
        public string BackgroundColor
        {
            get
            {
                if (IsOverdue) return "Black";
                if (_task.Status == TaskStatus.Completed) return "White";
                if (_task.Status == TaskStatus.AwaitingReview) return "LightSkyBlue";
                if (_task.Status == TaskStatus.NeedsWork) return "PaleGreen";

                var daysLeft = (_task.DueDate.Date - DateTime.Today.Date).Days;
                if (daysLeft > 30) return "Green";
                if (daysLeft <= 7) return "Red";
                return "Yellow";
            }
        }

        // Determines the foreground color of the icon.
        public string ForegroundColor
        {
            get
            {
                if (IsOverdue) return "Red";
                switch (_task.Status)
                {
                    case TaskStatus.Completed: return "Green";
                    case TaskStatus.AwaitingReview: return "Yellow";
                    case TaskStatus.NeedsWork: return "Yellow";
                    default: return "Black";
                }
            }
        }

        // Determines the border color of the task cell.
        public string BorderColor
        {
            get
            {
                if (_task.Status == TaskStatus.Completed) return "Green";
                if (IsOverdue) return "Red";
                return "DarkGray";
            }
        }

        // Checks if the task is overdue.
        private bool IsOverdue => _task.DueDate.Date < DateTime.Today.Date && _task.Status != TaskStatus.Completed;
    }
}