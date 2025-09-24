using System.Windows.Controls;
using System.Windows.Input;
using ClientApp.UI.ViewModels;

namespace ClientApp.UI.Views.UserControls
{
    public partial class CalendarView : UserControl
    {
        //Initialization.
        public CalendarView()
        {
            InitializeComponent();
        }

        // Handles mouse down events on task cells to trigger the selection command.
        private void TaskCellControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TaskCellControl control &&
                control.DataContext is TaskCellViewModel viewModel)
            {
                var calendarVM = (CalendarViewModel)DataContext;
                calendarVM.DaySelectedCommand.Execute(viewModel);
            }
        }
    }
}