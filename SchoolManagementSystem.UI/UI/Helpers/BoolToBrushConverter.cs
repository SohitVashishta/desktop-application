using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush ActiveBrush { get; set; } = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Green
        public Brush InactiveBrush { get; set; } = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Red

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
                return isActive ? ActiveBrush : InactiveBrush;

            return InactiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
