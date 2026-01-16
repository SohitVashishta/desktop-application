using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class ManageUsersPage : UserControl
    {
        public ManageUsersPage()
        {
            InitializeComponent();

            DataContext = App.Services.GetRequiredService<UserManagementViewModel>();

            Loaded += ManageUsersPage_Loaded;
        }

        private async void ManageUsersPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is UserManagementViewModel vm)
            {
                await vm.LoadOnStartupAsync();
            }
        }
    }
}
