using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Windows;
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

        private async void ManageUsersPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManagementViewModel vm)
            {
                await vm.LoadOnStartupAsync();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
