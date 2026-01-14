using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Windows;

namespace SchoolManagementSystem.UI.UI.Views.Students
{
    public partial class StudentAddEditView : Window
    {
        public StudentAddEditView()
        {
            InitializeComponent();
             DataContext = App.Services.GetRequiredService<StudentAddEditViewModel>();

        }

        public StudentAddEditView(Student student)
        {
            InitializeComponent();
        DataContext = App.Services.GetRequiredService<StudentAddEditViewModel>();

        }
    }
}
