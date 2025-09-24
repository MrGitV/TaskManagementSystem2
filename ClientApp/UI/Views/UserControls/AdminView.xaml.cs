using System.Windows.Controls;
using ClientApp.UI.ViewModels;

namespace ClientApp.UI.Views.UserControls
{
    public partial class AdminView : UserControl
    {
        //Initialization.
        public AdminView()
        {
            InitializeComponent();
        }

        // Handles selection changes in the users grid.
        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is UserViewModel user)
            {
                var viewModel = (AdminViewModel)DataContext;
                viewModel.OpenAssignmentCommand.Execute(user);
            }
        }
    }
}