using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.UI.UI.ViewModels.FeeManagement;
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

namespace SchoolManagementSystem.UI.UI.Views.FeeManagement
{
    /// <summary>
    /// Interaction logic for AddNewFeeEntry.xaml
    /// </summary>
    public partial class AddNewFeeEntry : UserControl
    {
        public AddNewFeeEntry()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<AddNewFeeEntryViewModel>(); ;

        }
    }
}
