using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Kurai.Launcher.Pages;

public partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
    }


    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var animation = new ThicknessAnimation
        {
            To = new Thickness(0,StackPanelTest.Children.IndexOf((UIElement)sender) * -313,0,0),
            Duration = TimeSpan.FromMilliseconds(200),
            EasingFunction = new QuadraticEase{ EasingMode = EasingMode.EaseInOut }
        };
        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, PagesStackPanel);
        Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin(PagesStackPanel);
    }

    private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        RadioButton1.IsChecked = true;
    }
}