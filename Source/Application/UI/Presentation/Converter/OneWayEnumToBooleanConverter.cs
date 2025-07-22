using System;
using System.Globalization;
using System.Windows.Data;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

internal class OneWayEnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && (bool)value == true)
        {
            var parameterString = parameter.ToString();
            return Enum.Parse(targetType, parameterString);
        }
        return -1;
    }
}
