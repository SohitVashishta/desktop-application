using SchoolManagementSystem.Business.Services;
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
    /// Interaction logic for BookInventoryPage.xaml
    /// </summary>
    public partial class BookInventoryPage : UserControl
    {
        public BookInventoryPage()
        {
            InitializeComponent();

            var repository = new BookRepository();
            var service = new BookService(repository);

            DataContext = new BookInventoryViewModel(service);
        }
    }
}
