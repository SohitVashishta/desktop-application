using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic;
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

namespace SchoolManagementSystem.UI.UI.Views.StudentManagement.Academic
{
    /// <summary>
    /// Interaction logic for SubjectMasterPage.xaml
    /// </summary>
    public partial class SubjectMasterPage : UserControl
    {
        public SubjectMasterPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<SubjectMasterViewModel>();

        }
    }
}
