using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class ColorOpacityModifier : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Color color = (Color)value;
        return Color.FromRgba(color.R, color.G, color.B, color.A * System.Convert.ToDouble(parameter));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Color color = (Color)value;
        return Color.FromRgba(color.R, color.G, color.B, color.A + color.A * System.Convert.ToDouble(parameter));
    }
}