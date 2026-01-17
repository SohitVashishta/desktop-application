using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class PasswordMismatchVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return Visibility.Collapsed;

            var password = values[0] as string;
            var confirmPassword = values[1] as string;

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                return Visibility.Collapsed;

            return password != confirmPassword
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
