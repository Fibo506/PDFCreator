using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pdfforge.PDFCreator.UI.Presentation.Styles;

public static class TextBoxBehavior
{
    public static readonly DependencyProperty TripleClickSelectAllProperty = DependencyProperty.RegisterAttached(
        "TripleClickSelectAll", typeof(bool), typeof(TextBoxBehavior), new PropertyMetadata(false, OnPropertyChanged));

    private static void OnPropertyChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs args)
    {
        var textBox = dependency as TextBox;
        if (textBox != null)
        {
            var enable = (bool)args.NewValue;
            if (enable)
            {
                textBox.PreviewMouseLeftButtonDown += OnTextBoxMouseDown;
            }
            else
            {
                textBox.PreviewMouseLeftButtonDown -= OnTextBoxMouseDown;
            }
        }
    }

    private static void OnTextBoxMouseDown(object sender, MouseButtonEventArgs args)
    {
        if (args.ClickCount == 3)
        {
            ((TextBox)sender).SelectAll();
        }
    }

    public static void SetTripleClickSelectAll(DependencyObject element, bool value)
    {
        element.SetValue(TripleClickSelectAllProperty, value);
    }

    public static bool GetTripleClickSelectAll(DependencyObject element)
    {
        return (bool)element.GetValue(TripleClickSelectAllProperty);
    }
}
