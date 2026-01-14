using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class ManageUsersPage : UserControl
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public ManageUsersPage()
        {
            InitializeComponent();

            Users = new ObservableCollection<UserModel>
            {
                new UserModel { Username="admin", Role="Admin", IsActive=true },
                new UserModel { Username="teacher1", Role="Teacher", IsActive=true }
            };

            DataContext = App.Services.GetRequiredService<UserManagementViewModel>();
        }

        private void AddUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Open Add User Dialog
        }

        private void EditUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Open Edit User Dialog
        }

        private void DeactivateUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Deactivate selected user
        }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
