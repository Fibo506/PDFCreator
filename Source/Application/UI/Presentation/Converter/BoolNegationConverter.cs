using System;
using System.Globalization;
using System.Windows.Data;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

[ValueConversion(typeof(bool), typeof(bool))]
[ValueConversion(typeof(bool?), typeof(bool))]
public class BoolNegationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (targetType == typeof(bool?))
            return !(bool?)value;

        if (targetType == typeof(bool))
            return !(bool)value;

        throw new InvalidOperationException("The target must be a boolean");
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        return Convert(value, targetType, parameter, culture);
    }
}
