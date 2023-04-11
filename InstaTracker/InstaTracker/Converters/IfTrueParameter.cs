using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IfTrueParameter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? parameter : null!;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not null;
}