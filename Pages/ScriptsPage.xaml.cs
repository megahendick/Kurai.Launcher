using System.Windows.Controls;
using System.Windows.Input;

namespace Kurai.Launcher.Pages;

public partial class ScriptsPage : Page
{
    public ScriptsPage()
    {
        InitializeComponent();
    }

    private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;
        if (e.Delta < 0)
        {
            scrollViewer.LineRight();
        }
        else
        {
            scrollViewer.LineLeft();
        }
        e.Handled = true;
    }
}