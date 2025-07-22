using System;
using System.Windows;
using Prism;

namespace pdfforge.PDFCreator.UI.Presentation.Controls.Tab;

/// <summary>
/// SubItems for selectable lists inside a tab header
///
/// Implementing IActiveAware allows us to emit an event when the item becomes active (selected) and navigate based upon that
/// </summary>
public class SubItem : DependencyObject, IActiveAware
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        "IsEnabled", typeof(bool), typeof(SubItem), new PropertyMetadata(default(bool)));

    public bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        "Text", typeof(string), typeof(SubItem), new PropertyMetadata(default(string)));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty NavigationUriProperty = DependencyProperty.Register(
        "NavigationUri", typeof(string), typeof(SubItem), new PropertyMetadata(default(string)));

    private bool _isActive;

    public string NavigationUri
    {
        get { return (string)GetValue(NavigationUriProperty); }
        set { SetValue(NavigationUriProperty, value); }
    }

    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string ReagionName { get; set; }

    public event EventHandler IsActiveChanged;
}
