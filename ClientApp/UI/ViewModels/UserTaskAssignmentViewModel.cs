using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClientApp.DTOs;
using ClientApp.Infrastructure;
using ClientApp.Services;
using ClientApp.UI.Views;

namespace ClientApp.UI.ViewModels
{
    // ViewModel for assigning tasks to a specific user.
    public class UserTaskAssignmentViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;

        // The user to whom tasks are being assigned.
        public UserDto User { get; }
        // A collection of all tasks with their assignment status for this user.
        public ObservableCollection<TaskAssignmentItem> Tasks { get; } = new ObservableCollection<TaskAssignmentItem>();

        // Command to save the assignment changes.
        public ICommand SaveCommand { get; }
        // Command to cancel and close the window.
        public ICommand CancelCommand { get; }

        // ViewModel constructor.
        public UserTaskAssignmentViewModel(UserDto user, ITaskService taskService)
        {
            User = user;
            _taskService = taskService;
            LoadTasks();

            SaveCommand = new RelayCommand(async _ => await SaveAssignments(), _ => Tasks.Any(t => t.IsDirty));
            CancelCommand = new RelayCommand(_ => CloseWindow());
        }

        // Loads all tasks and sets their initial assignment status.
        private async void LoadTasks()
        {
            var allTasks = await _taskService.GetAllTasksAsync();
            Tasks.Clear();
            foreach (var task in allTasks)
            {
                if (task.Status != DTOs.TaskStatus.Completed || task.AssignedToUserId == User.Id)
                {
                    Tasks.Add(new TaskAssignmentItem(task, task.AssignedToUserId == User.Id));
                }
            }
        }

        // Saves any changes made to task assignments.
        public async Task SaveAssignments()
        {
            var tasksToUpdate = Tasks.Where(t => t.IsDirty).ToList();
            foreach (var item in tasksToUpdate)
            {
                if (item.IsAssigned)
                {
                    item.Task.AssignedToUserId = User.Id;
                }
                else if (item.Task.AssignedToUserId == User.Id)
                {
                    item.Task.AssignedToUserId = null;
                }

                await _taskService.UpdateTaskAsync(item.Task);
            }
            CloseWindow();
        }

        // Closes the assignment window.
        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<UserTaskAssignmentWindow>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}