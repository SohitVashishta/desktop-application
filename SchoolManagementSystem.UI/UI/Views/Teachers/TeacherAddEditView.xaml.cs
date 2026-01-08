using System.Windows;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.ViewModels;

namespace SchoolManagementSystem.UI.UI.Views.Teachers
{
    public partial class TeacherAddEditView : Window
    {
        public TeacherAddEditView()
        {
            InitializeComponent();
            DataContext = new TeacherAddEditViewModel(this);
        }

        public TeacherAddEditView(Teacher teacher)
        {
            InitializeComponent();
            DataContext = new TeacherAddEditViewModel(this, teacher);
        }
    }
}
