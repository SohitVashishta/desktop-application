using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels;

namespace SchoolManagementSystem.UI.UI.Views.Dashboard
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            // ✅ Resolve ViewModel via DI
            DataContext = App.Services.GetRequiredService<DashboardViewModel>();
        }
    }
}
