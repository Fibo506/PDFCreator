using pdfforge.PDFCreator.UI.Interactions;
using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Dialogs;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

// ReSharper disable once ClassNeverInstantiated.Global
public class DesignTimeInputBoxWindowViewModel : InputBoxWindowViewModel
{
    public DesignTimeInputBoxWindowViewModel() : base(new DesignTimeTranslationUpdater())
    {
        //ConcreteInteraction.QuestionText = "";
        //Interaction = new InputInteraction("Please enter the text here:");
        SetInteraction(new InputInteraction("My Window Title", "Please enter the text here:"));

        Interaction.IsValidInput = s => new InputValidation(false, "This is a testing validation message.");
        Interaction.InputText = "This is my input text";
    }
}
