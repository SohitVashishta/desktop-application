using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Windows;

namespace SchoolManagementSystem.UI.UI.Views.Teachers
{
    public partial class TeacherAddEditView : Window
    {
        public TeacherAddEditView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<TeacherAddEditViewModel>();
        }

        
    }
}
