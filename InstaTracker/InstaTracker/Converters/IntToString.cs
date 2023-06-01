using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IntToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return "0";

        int dif = System.Convert.ToInt32(value);
        return dif > 0 ? $"+{dif}" : dif < 0 ? dif.ToString() : "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}