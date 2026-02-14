using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.UI.UI.ViewModels.Library;
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

namespace SchoolManagementSystem.UI.UI.Views.Library
{
    /// <summary>
    /// Interaction logic for IssueReturnHistoryPage.xaml
    /// </summary>
    public partial class IssueReturnHistoryPage : UserControl
    {
        public IssueReturnHistoryPage()
        {
            InitializeComponent();

            var repository = new BookTransactionRepository();
            var service = new BookTransactionService(repository);

            DataContext = new IssueReturnHistoryViewModel(service);
        }
    }
}
