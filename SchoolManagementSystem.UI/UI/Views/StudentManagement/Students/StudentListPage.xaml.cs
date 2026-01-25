using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.StudentManagement
{
    public partial class StudentListPage : UserControl
    {
        public StudentListPage()
        {
            InitializeComponent();

            DataContext = new StudentListViewModel(
                App.Services.GetRequiredService<IStudentService>(),
                App.Services.GetRequiredService<IClassService>(),
                App.Services.GetRequiredService<ISectionService>(),
                App.Services.GetRequiredService<IAcademicYearService>()
            );
        }
    }
}
