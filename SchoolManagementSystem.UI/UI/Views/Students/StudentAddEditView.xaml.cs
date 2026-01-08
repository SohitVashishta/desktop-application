using System.Windows;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.ViewModels;

namespace SchoolManagementSystem.UI.UI.Views.Students
{
    public partial class StudentAddEditView : Window
    {
        public StudentAddEditView()
        {
            InitializeComponent();
            DataContext = new StudentAddEditViewModel(this);
        }

        public StudentAddEditView(Student student)
        {
            InitializeComponent();
            DataContext = new StudentAddEditViewModel(this, student);
        }
    }
}
