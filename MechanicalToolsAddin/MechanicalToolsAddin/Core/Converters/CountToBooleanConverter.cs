using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MechanicalToolsAddin
{
    public class CountToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
                return false;

    
            foreach (var value in values)
            {
                if (value is int count && count > 0)
                    return true;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}