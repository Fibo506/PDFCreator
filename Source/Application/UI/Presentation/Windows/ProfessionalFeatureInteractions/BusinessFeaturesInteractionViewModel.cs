using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.UI.Presentation.Commands;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using Prism.Commands;

namespace pdfforge.PDFCreator.UI.Presentation.Windows.ProfessionalFeatureInteractions;

public class BusinessFeaturesInteractionViewModel : OverlayViewModelBase<BusinessFeaturesUserInteraction, BusinessFeaturesTranslatable>
{
    public event EventHandler ClosingRequest;
    public DelegateCommand CloseWindowCommand { get; }
    public ICommand OpenUrlCommand { get; }
    public ObservableCollection<FeatureRow> Rows { get; }

    public BusinessFeaturesInteractionViewModel(ITranslationUpdater translationUpdater, ICommandLocator commandLocator) : base(translationUpdater)
    {
        CloseWindowCommand = new DelegateCommand(CloseWindowExecute);
        OpenUrlCommand = commandLocator.GetCommand<UrlOpenCommand>();

        Rows =
        [
            new FeatureRow { Text = Translation.Row1Feature },
            new FeatureRow { Text = Translation.Row2Feature },
            new FeatureRow { Text = Translation.Row3Feature },
            new FeatureRow { Text = Translation.Row4Feature },
            new FeatureRow { Text = Translation.Row5Feature },
            new FeatureRow { Text = Translation.Row6Feature },
            new FeatureRow { Text = Translation.Row7Feature },
            new FeatureRow { Text = Translation.Row8Feature },
            new FeatureRow { Text = Translation.Row9Feature }
        ];

        Title = "";
    }

    public override string Title { get; }

    private void CloseWindowExecute()
    {
        ClosingRequest?.Invoke(this, EventArgs.Empty);
    }
}

public class FeatureRow
{
    public string Text { get; set; }
}
