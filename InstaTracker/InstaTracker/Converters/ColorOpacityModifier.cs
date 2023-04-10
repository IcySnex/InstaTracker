using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class OpacitySwitcher : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? 1.0 : System.Convert.ToDouble(parameter);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        (double)value == 1.0;
}