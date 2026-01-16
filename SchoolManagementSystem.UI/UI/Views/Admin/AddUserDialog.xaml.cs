using System.Windows;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class AddUserDialog : Window
    {
        public AddUserDialog(AddUserViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            // 🔹 Hook password manually
            vm.PasswordProvider = () => PwdBox.Password;
        }
    }
}
