using ClientApp.DTOs;

namespace ClientApp.UI.ViewModels
{
    // Represents an item in a task assignment list, tracking its assignment status.
    public class TaskAssignmentItem : ViewModelBase
    {
        public TaskDto Task { get; }
        private bool _isAssigned;
        private readonly bool _originalIsAssigned;

        // Gets or sets whether the task is assigned to the current user.
        public bool IsAssigned
        {
            get => _isAssigned;
            set
            {
                if (SetProperty(ref _isAssigned, value))
                {
                    OnPropertyChanged(nameof(IsDirty));
                }
            }
        }

        // Indicates if the assignment status of the task has changed from its original state.
        public bool IsDirty => _isAssigned != _originalIsAssigned;

        // Initializes a new instance of the TaskAssignmentItem class.
        public TaskAssignmentItem(TaskDto task, bool isAssigned)
        {
            Task = task;
            _isAssigned = isAssigned;
            _originalIsAssigned = isAssigned;
        }
    }
}