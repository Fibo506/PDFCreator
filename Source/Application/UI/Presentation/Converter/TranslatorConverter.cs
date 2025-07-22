using System;
using System.Globalization;
using System.Windows.Data;
using pdfforge.PDFCreator.Core.ServiceLocator;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.Styles.Gpo;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.Converter;

public class TranslatorConverter : IValueConverter
{
    private GpoTranslation _translation = new GpoTranslation();
    private readonly ApplicationNameProvider _applicationNameProvider;

    public TranslatorConverter()
    {
        if (RestrictedServiceLocator.IsLocationProviderSet)
        {
            var translationUpdater = RestrictedServiceLocator.Current.GetInstance<ITranslationUpdater>();
            translationUpdater.RegisterAndSetTranslation(tf => _translation = tf.UpdateOrCreateTranslation(_translation));

            _applicationNameProvider = RestrictedServiceLocator.Current.GetInstance<ApplicationNameProvider>();
        }
    }

    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (_applicationNameProvider.EditionName == "Free")
        {
            return _translation.NotAvailableOnFree;
        }
        return _translation.SetByAdministrator;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
