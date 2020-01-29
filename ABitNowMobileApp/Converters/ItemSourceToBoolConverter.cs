using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ABitNowMobileApp.Converters
{
    public class ItemSourceToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            IList<object> list = (IList<object>)value;
            return list != null && list.Count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
