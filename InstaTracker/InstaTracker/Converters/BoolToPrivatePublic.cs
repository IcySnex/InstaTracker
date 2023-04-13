using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class BoolToPrivatePublic : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? "Private" : "Public";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        (string)value == "Private";
}