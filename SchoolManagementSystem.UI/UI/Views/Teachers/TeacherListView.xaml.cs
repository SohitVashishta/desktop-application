using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Teachers
{
    public partial class TeacherListView : UserControl
    {
        public TeacherListView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<TeacherViewModel>();

        }
    }
}
