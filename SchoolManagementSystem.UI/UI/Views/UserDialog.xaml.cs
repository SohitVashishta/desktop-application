using SchoolManagementSystem.Business.Services;
using System.Windows;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class UserDialog : Window
    {
        public UserDialog(IUserService userService)
        {
            InitializeComponent();
            DataContext = new UserDialogViewModel(userService); // 🔥 REQUIRED
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDialogViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

    }
}
