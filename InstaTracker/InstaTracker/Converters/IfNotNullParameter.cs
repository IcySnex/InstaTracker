using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IfNotNullParameter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null ? null! : parameter;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}