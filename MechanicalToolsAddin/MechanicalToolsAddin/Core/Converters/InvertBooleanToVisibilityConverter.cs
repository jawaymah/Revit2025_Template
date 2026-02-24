using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MechanicalToolsAddin
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public bool UseHidden { get; set; } = false; // Optional: use Visibility.Hidden instead of Collapsed

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
            {
                return boolean
                    ? (UseHidden ? Visibility.Hidden : Visibility.Collapsed)
                    : Visibility.Visible;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility != Visibility.Visible;
            }

            return false;
        }
    }

}
