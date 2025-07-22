using System.Windows;
using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;

public class BindableTextBox : TextBox
{
    public static DependencyProperty BindableSelectionStartProperty = DependencyProperty.Register(
        "BindableSelectionStart",
        typeof(int),
        typeof(BindableTextBox),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableSelectionStartChanged));

    public static DependencyProperty BindableSelectionLengthProperty = DependencyProperty.Register(
        "BindableSelectionLength",
        typeof(int),
        typeof(BindableTextBox),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableSelectionLengthChanged));

    private bool _changeFromUi;

    public BindableTextBox()
    {
        SelectionChanged += OnSelectonChanged;
        Loaded += (sender, args) => SelectionStart = Text.Length;
    }

    public int BindableSelectionLength
    {
        get { return (int)GetValue(BindableSelectionLengthProperty); }

        set { SetValue(BindableSelectionLengthProperty, value); }
    }

    public int BindableSelectionStart
    {
        get { return (int)GetValue(BindableSelectionStartProperty); }

        set { SetValue(BindableSelectionStartProperty, value); }
    }

    private static void OnBindableSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var textBox = d as BindableTextBox;

        if (!textBox._changeFromUi)
        {
            var newValue = (int)e.NewValue;
            textBox.SelectionStart = newValue;
        }
        else
        {
            textBox._changeFromUi = false;
        }
    }

    private static void OnBindableSelectionLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var textBox = d as BindableTextBox;

        if (!textBox._changeFromUi)
        {
            var newValue = (int)e.NewValue;
            textBox.SelectionLength = newValue;
        }
        else
        {
            textBox._changeFromUi = false;
        }
    }

    private void OnSelectonChanged(object sender, RoutedEventArgs e)
    {
        if (BindableSelectionStart != SelectionStart)
        {
            _changeFromUi = true;
            BindableSelectionStart = SelectionStart;
        }

        if (BindableSelectionLength != SelectionLength)
        {
            _changeFromUi = true;
            BindableSelectionLength = SelectionLength;
        }
    }
}
