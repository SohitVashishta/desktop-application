using System.Windows;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public static class MenuHelper
    {
        public static readonly DependencyProperty IsActiveProperty =
    DependencyProperty.RegisterAttached(
        "IsActive",
        typeof(bool),
        typeof(MenuHelper),
        new PropertyMetadata(false));

        public static bool GetIsActive(DependencyObject obj)
            => (bool)obj.GetValue(IsActiveProperty);

        public static void SetIsActive(DependencyObject obj, bool value)
            => obj.SetValue(IsActiveProperty, value);

    }
}
