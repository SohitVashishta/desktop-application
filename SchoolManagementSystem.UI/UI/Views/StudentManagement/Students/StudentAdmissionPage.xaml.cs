using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolManagementSystem.UI.UI.Views.StudentManagement
{
    /// <summary>
    /// Interaction logic for StudentAdmissionPage.xaml
    /// </summary>
    public partial class StudentAdmissionPage : UserControl
    {
        public StudentAdmissionPage()
        {
            InitializeComponent();
            DataContext = new StudentAdmissionViewModel(
        App.Services.GetRequiredService<IStudentService>(),
        App.Services.GetRequiredService<IAcademicYearService>(),
        App.Services.GetRequiredService<IClassService>(),
        App.Services.GetRequiredService<ISectionService>()
    );
        }
    }
}
