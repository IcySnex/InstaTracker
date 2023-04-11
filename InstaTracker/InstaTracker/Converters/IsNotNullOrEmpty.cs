using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IsNotNullOrEmpty : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        !string.IsNullOrEmpty((string)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        null!;
}