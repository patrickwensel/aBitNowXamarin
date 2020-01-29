using System;
using System.Globalization;
using Xamarin.Forms;

namespace ABitNowMobileApp.Converters
{
    public class TopThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double otherSideDefaultThicknessValue = 0;
            if (value == null)
                return new Thickness(otherSideDefaultThicknessValue, 10, otherSideDefaultThicknessValue, otherSideDefaultThicknessValue);

            if (parameter != null)
                double.TryParse(parameter.ToString(), out otherSideDefaultThicknessValue);

            double topThickness = (double)value;
            return new Thickness(otherSideDefaultThicknessValue, topThickness, otherSideDefaultThicknessValue, otherSideDefaultThicknessValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
