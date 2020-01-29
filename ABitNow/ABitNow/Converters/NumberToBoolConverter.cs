using System;
using System.Globalization;
using Xamarin.Forms;

namespace ABitNow.Converters
{
    public class NumberToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            double minimumValue = 0;
            if (parameter != null)
                double.TryParse(parameter.ToString(), out minimumValue);

            double actualValue = (double)value;

            return actualValue > minimumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
