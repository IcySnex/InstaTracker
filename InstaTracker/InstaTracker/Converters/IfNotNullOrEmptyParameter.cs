using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IfNotNullOrEmptyParameter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        string.IsNullOrEmpty((string)value) ? null! : parameter;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}