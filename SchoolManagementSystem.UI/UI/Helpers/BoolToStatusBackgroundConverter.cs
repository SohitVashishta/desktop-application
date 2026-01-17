using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class BoolToStatusBackgroundConverter : IValueConverter
    {
        private static readonly Brush ActiveBrush =
            new SolidColorBrush(Color.FromRgb(34, 197, 94));   // Green

        private static readonly Brush InactiveBrush =
            new SolidColorBrush(Color.FromRgb(239, 68, 68));  // Red

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isActive && isActive
                ? ActiveBrush
                : InactiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
