using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Kurai.Launcher.Styles;
using Newtonsoft.Json;

namespace Kurai.Launcher.Pages;

public partial class ScriptsPage : Page
{
    private Root _myDeserializedClass;
    public static List<string> _selectedTags = [];
    public ScriptsPage()
    {
        InitializeComponent();
        
        var tags = new List<string> { "Installed", "Combat", "PvP", "PvE", "Hive" };
        foreach (var tb in tags.Select(tag => new ToggleButton { Content = tag }))
        {
            tb.Click += TbOnClick;
            TagsStackPanel.Children.Add(tb);
        }
    }
    
    private void TbOnClick(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton toggleButton)
            if (toggleButton.IsChecked ?? false)
                _selectedTags.Add((string)toggleButton.Content);
            else
                _selectedTags.Remove((string)toggleButton.Content);
     
        StackPanel1.Children.Clear();
        StackPanel2.Children.Clear();
        UpdateLayout();
        
        foreach (var script in _myDeserializedClass.Scripts)
        {
            //checks if script title contains searchbox (except if searchbox text is blank)
            if (!string.IsNullOrEmpty(SearchBox.Text) && !script.Name.Contains(SearchBox.Text, StringComparison.CurrentCultureIgnoreCase)) continue;
            
            //checks if script contains every selected tag
            if (_selectedTags.Count > 0 && _selectedTags.Any(tag => !script.Tags.Contains(tag))) continue;

            var sp = StackPanel1;
            if (StackPanel1.ActualHeight > StackPanel2.ActualHeight) sp = StackPanel2;
            sp.Children.Add(new ScriptItem
            {
                Title = script.Name,
                Author = script.Author,
                Body = script.Body,
                Tags = script.Tags
            });
            UpdateLayout();
        }
    }

    private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;
        if (e.Delta < 0)
            scrollViewer.LineRight();
        else
            scrollViewer.LineLeft();
        e.Handled = true;
    }

    private async void ScriptsPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        string s;
        using (var client = new HttpClient()) s = await client.GetStringAsync("https://raw.githubusercontent.com/megahendick/Lyra.Launcher.Testing/main/scripts");
        
        _myDeserializedClass = JsonConvert.DeserializeObject<Root>(s);
        foreach (var script in _myDeserializedClass.Scripts)
        {
            var sp = StackPanel1;
            if (StackPanel1.ActualHeight > StackPanel2.ActualHeight) sp = StackPanel2;
            sp.Children.Add(new ScriptItem
            {
                Title = script.Name,
                Author = script.Author,
                Body = script.Body,
                Tags = script.Tags
            });
            UpdateLayout();
        }
    }
}

public class Root
{
    public List<Script> Scripts { get; set; }
}

public class Script
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
}
