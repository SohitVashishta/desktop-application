using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public static class NavigationService
    {
        public static Frame MainFrame { get; set; }

        public static void Navigate(Page page)
        {
            MainFrame?.Navigate(page);
        }
    }
}
