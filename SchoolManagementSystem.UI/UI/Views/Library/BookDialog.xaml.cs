using SchoolManagementSystem.UI.UI.ViewModels.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolManagementSystem.UI.UI.Views.Library
{
    /// <summary>
    /// Interaction logic for BookDialog.xaml
    /// </summary>
    public partial class BookDialog : Window
    {
        public BookDialog()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (DataContext is BookDialogViewModel vm)
                {
                    vm.CloseAction = Close;
                    vm.RequestFocusOnError = FocusFirstInvalidField;
                }
            };
        }
        private void FocusFirstInvalidField()
        {
            if (Validation.GetHasError(txtBookCode))
                txtBookCode.Focus();
            else if (Validation.GetHasError(txtTitle))
                txtTitle.Focus();
            else if (Validation.GetHasError(txtTotalCopies))
                txtTotalCopies.Focus();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
