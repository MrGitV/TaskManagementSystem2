using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;
using ClientApp.UI.ViewModels.Dialogs;
using ClientApp.UI.Views;

namespace ClientApp.UI.ViewModels
{
    // ViewModel for the task details window.
    public class TaskDetailsViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private object _selectedListItem;
        private TaskDto _selectedTask;
        private string _newCommentText;
        private string _statusMessage;
        private Brush _statusMessageColor;
        private string _dueTime;

        // Task due time in "HH:mm" format.
        public string DueTime { get => _dueTime; set => SetProperty(ref _dueTime, value); }
        // The date for which tasks are displayed.
        public DateTime Date { get; }
        // The list of tasks and the "[+] Add New Task" item.
        public ObservableCollection<object> TaskList { get; } = new ObservableCollection<object>();
        // Comments for the selected task.
        public ObservableCollection<CommentDto> Comments { get; } = new ObservableCollection<CommentDto>();

        // The selected item in the task list.
        public object SelectedListItem
        {
            get => _selectedListItem;
            set
            {
                if (SetProperty(ref _selectedListItem, value))
                {
                    if (value is TaskDto task)
                        SelectedTask = task;
                    else if (value as string == TranslationSource.Instance["AddNewTaskPlaceholder"])
                        AddNewTask();
                }
            }
        }

        // The selected task for editing.
        public TaskDto SelectedTask
        {
            get => _selectedTask;
            private set
            {
                if (SetProperty(ref _selectedTask, value))
                {
                    OnPropertyChanged(nameof(IsTaskCompleted));
                    OnPropertyChanged(nameof(IsDetailsEnabled));
                    LoadCommentsForSelectedTask();
                    DueTime = _selectedTask?.DueDate.ToString("HH:mm");
                }
            }
        }

        // The window title.
        public string TasksOnDateHeader => $"{TranslationSource.Instance["TasksOnDate"]} {Date:dd.MM.yyyy}";
        // Text for the new comment.
        public string NewCommentText { get => _newCommentText; set => SetProperty(ref _newCommentText, value); }
        // Status message for the user.
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
        // Color of the status message.
        public Brush StatusMessageColor { get => _statusMessageColor; set => SetProperty(ref _statusMessageColor, value); }
        // Whether the user is an administrator.
        public bool IsAdmin => _authService.CurrentRole == "Admin";
        // Whether the task details panel is enabled for editing.
        public bool IsDetailsEnabled => SelectedTask != null;

        // Task completion status.
        public bool IsTaskCompleted
        {
            get => SelectedTask?.Status == DTOs.TaskStatus.Completed;
            set
            {
                if (SelectedTask == null) return;
                SelectedTask.Status = value ? DTOs.TaskStatus.Completed : DTOs.TaskStatus.InProgress;
                OnPropertyChanged();
            }
        }

        // Commands.
        public ICommand DeleteTaskCommand { get; }
        public ICommand AddCommentCommand { get; }
        public ICommand SaveTaskCommand { get; }
        public ICommand CloseCommand { get; }

        // ViewModel constructor.
        public TaskDetailsViewModel(DateTime date, IEnumerable<TaskDto> tasks, ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
            Date = date;

            foreach (var task in tasks) TaskList.Add(task);
            if (IsAdmin)
            {
                TaskList.Add(TranslationSource.Instance["AddNewTaskPlaceholder"]);
            }

            SelectedTask = tasks.FirstOrDefault();

            DeleteTaskCommand = new RelayCommand(async param => await DeleteTaskAsync(param as TaskDto));
            AddCommentCommand = new RelayCommand(async _ => await AddCommentAsync(), _ => SelectedTask != null && SelectedTask.Id != 0);
            SaveTaskCommand = new RelayCommand(async _ => await SaveTaskAsync(), _ => CanSaveTask());
            CloseCommand = new RelayCommand(_ => CloseWindow());
        }

        // Loads comments for the selected task.
        private async void LoadCommentsForSelectedTask()
        {
            if (SelectedTask == null || SelectedTask.Id == 0)
            {
                Comments.Clear();
                return;
            }
            try
            {
                var comments = await _taskService.GetCommentsForTaskAsync(SelectedTask.Id);
                Comments.Clear();
                foreach (var comment in comments) Comments.Add(comment);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading comments: {ex.Message}";
                StatusMessageColor = Brushes.Red;
            }
        }

        // Adds a new comment.
        private async Task AddCommentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText) || SelectedTask == null || SelectedTask.Id == 0) return;
            try
            {
                var newComment = new CreateCommentRequest
                {
                    Text = NewCommentText,
                    TaskId = SelectedTask.Id
                };
                await _taskService.AddCommentAsync(newComment);
                LoadCommentsForSelectedTask();
                NewCommentText = string.Empty;
                StatusMessage = TranslationSource.Instance["CommentAddedSuccess"];
                StatusMessageColor = Brushes.Green;
            }
            catch (Exception ex)
            {
                StatusMessage = $"{TranslationSource.Instance["CommentAddError"]}: {ex.Message}";
                StatusMessageColor = Brushes.Red;
            }
        }

        // Initiates adding a new task.
        private void AddNewTask()
        {
            var newTask = new TaskDto
            {
                Title = TranslationSource.Instance["NewTaskTitle"],
                CreationDate = DateTime.Now,
                DueDate = Date.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute),
                Status = DTOs.TaskStatus.InProgress,
                ProjectId = 1
            };

            TaskList.Insert(TaskList.Count - 1, newTask);
            SelectedListItem = newTask;
        }

        // Deletes the selected task.
        private async Task DeleteTaskAsync(TaskDto taskToDelete)
        {
            if (taskToDelete == null) return;

            var message = string.Format(TranslationSource.Instance["ConfirmDeleteTaskMessage"], taskToDelete.Title);
            var caption = TranslationSource.Instance["ConfirmDeletionCaption"];
            var dialogViewModel = new ConfirmationDialogViewModel(caption, message);
            var dialog = new ConfirmationDialog(dialogViewModel)
            {
                Owner = Application.Current.Windows.OfType<TaskDetailsWindow>().FirstOrDefault(w => w.DataContext == this)
            };

            if (dialog.ShowDialog() == true)
            {
                if (taskToDelete.Id > 0)
                {
                    await _taskService.DeleteTaskAsync(taskToDelete.Id);
                }
                TaskList.Remove(taskToDelete);
                SelectedTask = TaskList.OfType<TaskDto>().FirstOrDefault();
            }
        }

        // Checks if the task can be saved.
        private bool CanSaveTask() => SelectedTask != null && !string.IsNullOrWhiteSpace(SelectedTask.Title) && IsAdmin;

        // Saves changes to the task.
        private async Task SaveTaskAsync()
        {
            if (!CanSaveTask()) return;
            try
            {
                var datePart = SelectedTask.DueDate.Date;
                if (TimeSpan.TryParseExact(DueTime, "g", CultureInfo.CurrentCulture, out TimeSpan timePart))
                {
                    SelectedTask.DueDate = datePart + timePart;
                }
                else
                {
                    StatusMessage = "Invalid time format. Please use HH:mm.";
                    StatusMessageColor = Brushes.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error parsing time: {ex.Message}";
                StatusMessageColor = Brushes.Red;
                return;
            }

            try
            {
                if (SelectedTask.Id == 0)
                {
                    int newId = await _taskService.AddTaskAsync(new CreateTaskRequest
                    {
                        Title = SelectedTask.Title,
                        Description = SelectedTask.Description,
                        DueDate = SelectedTask.DueDate,
                        Status = SelectedTask.Status,
                        AssignedToUserId = SelectedTask.AssignedToUserId,
                        ProjectId = SelectedTask.ProjectId
                    });
                    SelectedTask.Id = newId;
                    StatusMessage = TranslationSource.Instance["TaskAddedSuccess"];
                }
                else
                {
                    await _taskService.UpdateTaskAsync(SelectedTask);
                    StatusMessage = TranslationSource.Instance["TaskUpdatedSuccess"];
                }

                StatusMessageColor = Brushes.Green;
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                StatusMessage = $"{TranslationSource.Instance["TaskSaveError"]}: {ex.Message}";
                StatusMessageColor = Brushes.Red;
            }
        }

        // Closes the window.
        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<TaskDetailsWindow>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}