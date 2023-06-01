using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IsListNullOrEmpty : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is IEnumerable<object> list ? !list.Any() : true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value;
}