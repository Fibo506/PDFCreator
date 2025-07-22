using System;
using System.Globalization;
using System.Windows.Data;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

[ValueConversion(typeof(int), typeof(int))]
public sealed class CompareWithTresholdToValueConverter : IValueConverter
{
    public CompareWithTresholdToValueConverter()
    {
        // set defaults
        Treshold = 0;
        SmallerOrEqualValue = 0;
        BiggerValue = 0;
    }

    public int Treshold { get; set; }
    public int SmallerOrEqualValue { get; set; }
    public int BiggerValue { get; set; }

    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        try
        {
            return (int)value > Treshold ? BiggerValue : SmallerOrEqualValue;
        }
        catch
        {
            return 0;
        }
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        return null;
    }
}
