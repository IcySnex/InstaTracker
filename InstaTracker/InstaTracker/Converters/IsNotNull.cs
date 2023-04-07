using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IsNotNull : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null;
}