using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class IntIsNot : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (int)value != System.Convert.ToInt32(parameter);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        (int)value == System.Convert.ToInt32(parameter);
}