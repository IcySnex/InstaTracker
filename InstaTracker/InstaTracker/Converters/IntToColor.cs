using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IntToColor : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return Color.Transparent;

        int dif = System.Convert.ToInt32(value);
        return dif > 0 ? Color.FromRgb(140, 255, 157) : dif < 0 ? Color.FromRgb(255, 140, 140) : Color.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}