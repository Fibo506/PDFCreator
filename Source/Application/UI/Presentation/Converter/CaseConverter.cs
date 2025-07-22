using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

/// <summary>
/// From http://stackoverflow.com/a/29841405
/// </summary>
public class CaseConverter : IValueConverter
{
    private CultureInfo _culture;
    public CharacterCasing Case { get; set; }

    public CaseConverter()
    {
        Case = CharacterCasing.Upper;
        _culture = CultureInfo.CurrentCulture;
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var str = value as string;
        if (str != null)
        {
            switch (Case)
            {
                case CharacterCasing.Lower:
                    return str.ToLower(_culture);

                case CharacterCasing.Normal:
                    return str;

                case CharacterCasing.Upper:
                    return str.ToUpper(_culture);

                default:
                    return str;
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
