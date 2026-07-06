using System.Windows;

namespace pdfforge.PDFCreator.UI.Presentation.Windows.ProfessionalFeatureInteractions;
public partial class BusinessFeaturesView : Window
{
    public BusinessFeaturesView(BusinessFeaturesInteractionViewModel dataContext)
    {
        DataContext = dataContext;
        dataContext.ClosingRequest += (sender, e) => Close();
        InitializeComponent();
    }
}
