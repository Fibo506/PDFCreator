using System;
using System.Globalization;
using System.Windows.Data;
using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;
public class UpdateIntervalToEnabledConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length >= 2 &&
            values[0] is UpdateInterval interval &&
            values[1] is bool isFreeEdition)
        {
            // Disable "Never" option for free edition
            if (interval == UpdateInterval.Never && isFreeEdition)
                return false;
        }
        return true;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
