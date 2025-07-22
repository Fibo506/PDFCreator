using Prism.Mvvm;

namespace pdfforge.PDFCreator.UI.Presentation.Wrapper;

public class AskSwitchPrinter : BindableBase
{
    private bool _value;
    private string _name;

    public AskSwitchPrinter(string name, bool value)
    {
        Value = value;
        Name = name;
    }

    public bool Value
    {
        get { return _value; }
        set { _value = value; RaisePropertyChanged(); }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; RaisePropertyChanged(); }
    }
}
