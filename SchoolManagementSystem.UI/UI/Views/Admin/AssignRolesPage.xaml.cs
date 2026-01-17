using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class AssignRolesPage : UserControl
    {
        public AssignRolesPage()
        {
            InitializeComponent();

            // 🔴 THIS WAS MISSING
            DataContext = App.Services.GetRequiredService<AssignRolesViewModel>();

            Loaded += AssignRolesPage_Loaded;
        }

        private async void AssignRolesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AssignRolesViewModel vm)
            {
                await vm.LoadOnStartupAsync(); // ✅ now it WILL run
            }
        }
    }
}
