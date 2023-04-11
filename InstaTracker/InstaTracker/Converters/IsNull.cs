using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IsNull : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}