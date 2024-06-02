using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Kurai.Launcher;

[TemplatePart(Name = "TitleTextBlock", Type = typeof(TextBlock))]
[TemplatePart(Name = "BodyTextBlock", Type = typeof(TextBlock))]
public partial class NewsItem : RadioButton
{
    private TextBlock titleTextBlock;
    private TextBlock bodyTextBlock;
    private TextBlock authorTextBlock;
    private TextBlock dateTextBlock;
    
    private bool temp;
    
    public NewsItem()
    {
        InitializeComponent();
        DataContext = this;
    }
    
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
 
        // Code to get the Template parts as instance member
        titleTextBlock = GetTemplateChild("TitleTextBlock") as TextBlock;
        bodyTextBlock = GetTemplateChild("BodyTextBlock") as TextBlock;
        authorTextBlock = GetTemplateChild("AuthorTextBlock") as TextBlock;
        dateTextBlock = GetTemplateChild("DateTextBlock") as TextBlock;
    }
    
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title",
            typeof(string),
            typeof(NewsItem),
            new PropertyMetadata("Placeholder"));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    
    public static readonly DependencyProperty BodyProperty =
        DependencyProperty.Register("Body",
            typeof(string),
            typeof(NewsItem),
            new PropertyMetadata("Placeholder"));

    public string Body
    {
        get => (string)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }
    
    
    public static readonly DependencyProperty AuthorProperty =
        DependencyProperty.Register("Author",
            typeof(string),
            typeof(NewsItem),
            new PropertyMetadata("Placeholder"));

    public string Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }
    
    
    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date",
            typeof(string),
            typeof(NewsItem),
            new PropertyMetadata("Placeholder"));

    public string Date
    {
        get => (string)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
    
    
    public static readonly DependencyProperty ShowImageProperty =
        DependencyProperty.Register("ShowImage",
            typeof(bool),
            typeof(NewsItem),
            new PropertyMetadata(false));

    public bool ShowImage
    {
        get => (bool)GetValue(ShowImageProperty);
        set => SetValue(ShowImageProperty, value);
    }
    
    private bool isChecked;
    private async void NewsItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (!isChecked)
        {
            authorTextBlock.Visibility = Visibility.Visible;
            dateTextBlock.Visibility = Visibility.Visible;
            ShowImage = false;
            var animation2 = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(50),
                To = 710
            };
            var storyboard2 = new Storyboard();
            Storyboard.SetTarget(animation2, this);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(WidthProperty));
            storyboard2.Children.Add(animation2);
            storyboard2.Begin(this);
            
            await Task.Delay(50);
            
            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(250),
                To = titleTextBlock.ActualHeight + bodyTextBlock.ActualHeight + 15,
                EasingFunction = new QuadraticEase{EasingMode = EasingMode.EaseIn}
            };
            var storyboard = new Storyboard();
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath(HeightProperty));
            storyboard.Children.Add(animation);
            storyboard.Begin(this);
            AnimateFontSize(titleTextBlock, 18);
            AnimateFontSize(authorTextBlock, 14);
            AnimateFontSize(dateTextBlock, 14);
            isChecked = true;
        }
        else
        {
            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(250),
                To = 80,
                EasingFunction = new QuadraticEase{EasingMode = EasingMode.EaseOut}
            };
            var animation2 = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(50),
                To = 350,
                BeginTime = TimeSpan.FromMilliseconds(250)
            };
        
            var storyboard = new Storyboard();
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath(HeightProperty));
            Storyboard.SetTarget(animation2, this);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(WidthProperty));
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation2);
            storyboard.Begin(this);
            isChecked = false;
            AnimateFontSize(titleTextBlock, 16);
            AnimateFontSize(authorTextBlock, 0.000001);
            AnimateFontSize(dateTextBlock, 0.000001);
            await Task.Delay(250);
            authorTextBlock.Visibility = Visibility.Collapsed;
            dateTextBlock.Visibility = Visibility.Collapsed;
            ShowImage = temp;
        }
    }

    private async void NewsItem_OnUnchecked(object sender, RoutedEventArgs e)
    {
        var animation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(250),
            To = 80,
            EasingFunction = new QuadraticEase{EasingMode = EasingMode.EaseOut}
        };
        var animation2 = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(50),
            To = 350,
            BeginTime = TimeSpan.FromMilliseconds(250)
        };
        
        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, this);
        Storyboard.SetTargetProperty(animation, new PropertyPath(HeightProperty));
        Storyboard.SetTarget(animation2, this);
        Storyboard.SetTargetProperty(animation2, new PropertyPath(WidthProperty));
        storyboard.Children.Add(animation);
        storyboard.Children.Add(animation2);
        storyboard.Begin(this);
        isChecked = false;
        
        AnimateFontSize(titleTextBlock, 16);
        AnimateFontSize(authorTextBlock, 0.000001);
        AnimateFontSize(dateTextBlock, 0.000001);
        await Task.Delay(250);
        authorTextBlock.Visibility = Visibility.Collapsed;
        dateTextBlock.Visibility = Visibility.Collapsed;
        ShowImage = temp;
    }

    private static void AnimateFontSize(TextBlock tb, double to)
    {
        var animation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(250),
            To = to
        };
        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, tb);
        Storyboard.SetTargetProperty(animation, new PropertyPath(FontSizeProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin(tb);
    }

    private void NewsItem_OnLoaded(object sender, RoutedEventArgs e)
    {
        temp = ShowImage;
    }
}