using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Admin
{
    public partial class UserManagementView : UserControl
    {
        public UserManagementView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<UserManagementViewModel>();
        }
    }
}
