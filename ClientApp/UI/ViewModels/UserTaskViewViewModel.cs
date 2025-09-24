using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;

namespace ClientApp.UI.ViewModels
{
    // View model for viewing tasks assigned to a user.
    public class UserTaskViewViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private TaskDto _selectedTask;
        private string _newCommentText;

        // A collection of tasks to be displayed.
        public ObservableCollection<TaskDto> Tasks { get; }
        // A collection of comments for the selected task.
        public ObservableCollection<CommentDto> Comments { get; } = new ObservableCollection<CommentDto>();

        // The currently selected task in the list.
        public TaskDto SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value) && value != null)
                    LoadCommentsForSelectedTask();
            }
        }

        // The text for a new comment to be added.
        public string NewCommentText { get => _newCommentText; set => SetProperty(ref _newCommentText, value); }
        // The command to add a new comment.
        public ICommand AddCommentCommand { get; }

        // Initializes the view model.
        public UserTaskViewViewModel(IEnumerable<TaskDto> tasks, ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
            Tasks = new ObservableCollection<TaskDto>(tasks);
            if (Tasks.Any()) SelectedTask = Tasks.First();

            AddCommentCommand = new RelayCommand(async _ => await AddCommentAsync(), _ => SelectedTask != null && SelectedTask.Id != 0);
        }

        // Loads comments for the currently selected task.
        private async void LoadCommentsForSelectedTask()
        {
            if (SelectedTask == null) return;
            var comments = await _taskService.GetCommentsForTaskAsync(SelectedTask.Id);
            Comments.Clear();
            foreach (var comment in comments) Comments.Add(comment);
        }

        // Adds a new comment to the selected task.
        private async Task AddCommentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText)) return;
            var newComment = new CreateCommentRequest
            {
                Text = NewCommentText,
                TaskId = SelectedTask.Id
            };
            await _taskService.AddCommentAsync(newComment);
            LoadCommentsForSelectedTask();
            NewCommentText = string.Empty;
        }
    }
}