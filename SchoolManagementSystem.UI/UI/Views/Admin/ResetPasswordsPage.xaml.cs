using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class ResetPasswordsPage : UserControl
    {
        public ResetPasswordsPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<ResetPasswordsViewModel>();
            Loaded += ResetPasswordsPage_Loaded;
        }

        private async void ResetPasswordsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ResetPasswordsViewModel vm)
                await vm.LoadOnStartupAsync();
        }
    }
}
