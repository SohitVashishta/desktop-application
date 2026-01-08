using System.Windows.Controls;
using SchoolManagementSystem.UI.UI.ViewModels;

namespace SchoolManagementSystem.UI.UI.Views.Dashboard
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}
