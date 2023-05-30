using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class DateTimeFormatter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        ((DateTime)value).ToString((string)parameter, CultureInfo.InvariantCulture);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        DateTime.Parse((string)value);
}