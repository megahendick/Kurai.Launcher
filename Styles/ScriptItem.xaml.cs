using System.Windows;
using System.Windows.Controls;

namespace Kurai.Launcher.Styles;

public partial class ScriptItem : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title",
            typeof(string),
            typeof(ScriptItem),
            new PropertyMetadata("Placeholder"));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly DependencyProperty AuthorProperty =
        DependencyProperty.Register("Author",
            typeof(string),
            typeof(ScriptItem),
            new PropertyMetadata("Placeholder"));

    public string Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }
    
    public static readonly DependencyProperty BodyProperty =
        DependencyProperty.Register("Body",
            typeof(string),
            typeof(ScriptItem),
            new PropertyMetadata("Placeholder"));

    public string Body
    {
        get => (string)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }
    
    public static readonly DependencyProperty TagsProperty =
        DependencyProperty.Register("Tags",
            typeof(List<string>),
            typeof(ScriptItem),
            new PropertyMetadata(new List<string>{"Placeholder"}));

    public List<string> Tags
    {
        get => (List<string>)GetValue(TagsProperty);
        set
        {
            InstallButton.Content = value.Contains("Installed") ? "Uninstall" : "Install";
            WrapPanel.Children.Clear();
            foreach (var tag in value)
            {
                WrapPanel.Children.Add(new Label{ Content = tag });
            }
            SetValue(TagsProperty, value);
        }
    }

    public ScriptItem()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void InstallButton_OnClick(object sender, RoutedEventArgs e)
    {
        var temp = Tags;
        if ((string)InstallButton.Content == "Install")
            temp.Add("Installed");
        else
            temp.Remove("Installed");
        Tags = temp;
    }
}