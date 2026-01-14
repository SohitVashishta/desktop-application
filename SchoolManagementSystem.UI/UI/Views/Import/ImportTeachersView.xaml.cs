using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.ViewModels.Import;
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

namespace SchoolManagementSystem.UI.UI.Views.Import
{
    /// <summary>
    /// Interaction logic for ImportTeachersView.xaml
    /// </summary>
    public partial class ImportTeachersView : UserControl
    {
        public ImportTeachersView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<ImportTeachersViewModel>();


        }
    }
}
