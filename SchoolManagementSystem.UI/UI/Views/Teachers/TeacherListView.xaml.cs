using System.Windows.Controls;
using SchoolManagementSystem.UI.UI.ViewModels;

namespace SchoolManagementSystem.UI.UI.Views.Teachers
{
    public partial class TeacherListView : UserControl
    {
        public TeacherListView()
        {
            InitializeComponent();
            DataContext = new TeacherViewModel();
        }
    }
}
