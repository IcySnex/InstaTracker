using InstaTracker.Components;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IfDateTimeMatches : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (DateTime)value == (DateTime)((Chip)parameter).Tag;

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? (DateTime)parameter : null;
}