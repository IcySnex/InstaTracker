using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class BoolToAddedRemoved : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int mod = System.Convert.ToInt32(parameter);
        return (bool)value ? Color.FromRgb(140 - mod, 255 - mod, 157 - mod) : Color.FromRgb(255 - mod, 140 - mod, 140 - mod);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        (Color)value == Color.FromRgb(140, 255, 157);
}