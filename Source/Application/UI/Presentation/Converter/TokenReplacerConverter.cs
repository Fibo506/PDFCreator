using System;
using System.Globalization;
using System.Windows.Data;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

public class TokenReplacerConverter : IMultiValueConverter
{
    public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            throw new NotImplementedException("This converter needs at least two values");

        var stringvalue = values[0] as string;
        var tokenReplacer = values[1] as TokenReplacer;
        if (stringvalue == null)
            throw new NotImplementedException("This converter needs a string ");

        if (tokenReplacer == null)
            throw new NotImplementedException("This converter needs a TokenReplacer");

        return tokenReplacer.ReplaceTokens(stringvalue);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class TokenReplacerConverterForFileName : TokenReplacerConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var convertedValue = base.Convert(values, targetType, parameter, culture) as string;
        return ValidName.MakeValidFileName(convertedValue);
    }
}

public class TokenReplacerConverterForFolderName : TokenReplacerConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var convertedValue = base.Convert(values, targetType, parameter, culture) as string;
        return ValidName.MakeValidFolderName(convertedValue);
    }
}

public class TokenReplacerConverterForFtpDir : TokenReplacerConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var convertedValue = base.Convert(values, targetType, parameter, culture) as string;
        return ValidName.MakeValidFtpPath(convertedValue);
    }
}

public class TokenReplacerConverterWithFooter : TokenReplacerConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)
            throw new NotImplementedException("This converter needs three values: a string for the text, a TokenReplacer and a footer string");

        var footerText = values[2] as string;
        if (footerText == null)
            throw new NotImplementedException("This converter needs three values: a string for the text, a TokenReplacer and a footer string");

        var convertedValue = base.Convert(values, targetType, parameter, culture) as string;
        return convertedValue + footerText;
    }
}

public class TokenReplacerConverterForDropboxSharedFolder : TokenReplacerConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var convertedValue = base.Convert(values, targetType, parameter, culture) as string;
        return ValidName.MakeValidFtpPath(convertedValue);
    }
}
