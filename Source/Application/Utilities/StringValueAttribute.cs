using System;

namespace pdfforge.PDFCreator.Utilities;

public class StringValueAttribute : Attribute
{
    private readonly string _value;

    public StringValueAttribute(string value)
    {
        _value = value;
    }

    public static string GetValue(object value)
    {
        var fi = value.GetType().GetField(value.ToString());

        var attributes = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        if (attributes != null && attributes.Length > 0)
            return attributes[0]._value;

        throw new ArgumentException(nameof(value));
    }
}
