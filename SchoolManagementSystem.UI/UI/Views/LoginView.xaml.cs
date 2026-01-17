using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.Views.Dashboard;
using System.Windows;

namespace SchoolManagementSystem.UI.UI.Views
{
    public partial class LoginView : Window
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<LoginViewModel>();

        }
        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (pwdBox.Visibility == Visibility.Visible)
            {
                txtPasswordVisible.Text = pwdBox.Password;
                pwdBox.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }
            else
            {
                pwdBox.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                pwdBox.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
            }
        }

        // ================= PASSWORD TO VM =================
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = pwdBox.Visibility == Visibility.Visible
                ? pwdBox.Password
                : txtPasswordVisible.Text;

            ViewModel.LoginCommand.Execute(null);
        }

       

        // ================= FORGOT PASSWORD =================
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Contact administrator to reset password.",
                            "Forgot Password",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
