using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MechanicalToolsAddin
{
    /// <summary>
    /// Converts boolean to Visibility with optional inverse parameter
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;
            bool inverse = parameter?.ToString()?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;

            if (inverse)
                boolValue = !boolValue;

            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverse = parameter?.ToString()?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;
            bool result = value is Visibility visibility && visibility == Visibility.Visible;

            return inverse ? !result : result;
        }
    }
}