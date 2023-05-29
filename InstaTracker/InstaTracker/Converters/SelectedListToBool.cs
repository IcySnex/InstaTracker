using InstaTracker.Types;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace InstaTracker.Converters;

class SelectedListToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SelectedList selectedList)
            return false;

        bool isTrue = selectedList.ToString() == (string)parameter;
        return isTrue;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Enum.TryParse((string)parameter, true, out SelectedList selectedList);
        return (bool)value ? selectedList : null;
    }
}