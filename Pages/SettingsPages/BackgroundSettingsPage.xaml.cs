using System.Windows;
using System.Windows.Controls;

namespace Kurai.Launcher.Pages.SettingsPages;

public partial class BackgroundSettingsPage : Page
{
    public BackgroundSettingsPage()
    {
        InitializeComponent();
    }

    private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        MainWindow.BackgroundSpeed = Utils.Common.ConvertToDouble(((TextBox)sender).Text);
    }

    private void TextBoxBase_OnTextChanged2(object sender, TextChangedEventArgs e)
    {
        MainWindow.BackgroundFps = Utils.Common.ConvertToDouble(((TextBox)sender).Text);
    }
}