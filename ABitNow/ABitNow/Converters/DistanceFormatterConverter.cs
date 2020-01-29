using System;
using System.Globalization;
using Xamarin.Forms;

namespace ABitNow.Converters
{
    public class DistanceFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            double actualValue = (double)value;

            return $"{actualValue:0} mile" + (actualValue > 1 ? "s" : string.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
