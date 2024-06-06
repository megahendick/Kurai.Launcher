using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Kurai.Launcher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static string CurrentVersion = "1.20.81";
    
    public static double BackgroundSpeed { get; set; }
    public static double BackgroundFps
    {
        get => 1000 / aTimer.Interval;
        set
        {
            if (value == 0)
            {
                aTimer.Interval = 0.000001;
                return;
            }
            aTimer.Interval = 1000 / value;
        }
    }

    private static System.Timers.Timer aTimer = new();
    Stopwatch Stopwatch = new();

    public MainWindow()
    {
        InitializeComponent();
        
        dele = new WinEventDelegate(WinEventProc);
        IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

        BackgroundSpeed = 0.75;
        BackgroundFps = 24;
        aTimer.Elapsed += DispatcherTimer_Tick;
        aTimer.Start();
        Stopwatch.Start();
        
        Dispatcher.InvokeAsync((Action)(() => Utils.DiscordRpc.Initialize()));
    }

    WinEventDelegate dele = null;

    delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    private const uint WINEVENT_OUTOFCONTEXT = 0;
    private const uint EVENT_SYSTEM_FOREGROUND = 3;

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    private string GetActiveWindowTitle()
    {
        const int nChars = 256;
        IntPtr handle = IntPtr.Zero;
        StringBuilder Buff = new StringBuilder(nChars);
        handle = GetForegroundWindow();

        return GetWindowText(handle, Buff, nChars) > 0 ? Buff.ToString() : null;
    }

    public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        if (GetActiveWindowTitle() == "Kurai Launcher")
        {
            aTimer.Start();
            Stopwatch.Start();
        }
        else
        {
            aTimer.Stop();
            Stopwatch.Stop();
        }
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs e)
    {
        try
        {
			Dispatcher.Invoke(() =>
			{
				TestEffect.Time +=  aTimer.Interval / 1000 * BackgroundSpeed;
				//TestEffect.Time =  Stopwatch.Elapsed.TotalSeconds * BackgroundSpeed;
			});
		}
        catch
        {
            // ignored
        }
    }

    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        aTimer.Stop();
        Stopwatch.Stop();
        
        DragMove();
        
        aTimer.Start();
        Stopwatch.Start();
    }

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (PinToggleButton.IsChecked ?? false)
            return;
        
        var animation = new ThicknessAnimation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            EasingFunction = new QuadraticEase{ EasingMode = EasingMode.EaseOut},
            To = new Thickness(25,25,25,25)
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, (FrameworkElement)sender);
        Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin((FrameworkElement)sender);
        
        AnimateScale(LaunchButton, 1, EasingMode.EaseOut);
        AnimateMargin(LaunchButton, new Thickness(25), EasingMode.EaseOut);
        PinToggleButton2.Visibility = Visibility.Hidden;
        PinToggleButton.Visibility = Visibility.Visible;
    }

    private async void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (PinToggleButton.IsChecked ?? false)
            return;
        var animation = new ThicknessAnimation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            EasingFunction = new QuadraticEase{ EasingMode = EasingMode.EaseIn},
            To = new Thickness(25,25,25,-235)
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, (FrameworkElement)sender);
        Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin((FrameworkElement)sender);
        
        AnimateScale(LaunchButton, 1.1, EasingMode.EaseOut);
        AnimateMargin(LaunchButton, new Thickness(25,175,25,25), EasingMode.EaseIn);

        await Task.Delay(350);
        if (((FrameworkElement)sender).Margin != new Thickness(25, 25, 25, -235)) return;
        PinToggleButton.Visibility = Visibility.Hidden;
        PinToggleButton2.Visibility = Visibility.Visible;
    }
    
    public static void AnimateScale(FrameworkElement border, double to, EasingMode easingMode)
    {
        var animationX = new DoubleAnimation()
        {
            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
            EasingFunction = new QuadraticEase() {EasingMode = easingMode},
            To = to
        };

        var animationY = animationX.Clone();

        var storyboard = new Storyboard();
        storyboard.Children.Add(animationX);
        storyboard.Children.Add(animationY);
        Storyboard.SetTarget(animationX, border);
        Storyboard.SetTarget(animationY, border);
        Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
        Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));
    
        storyboard.Begin(border);
    }

    public static void AnimateMargin(FrameworkElement element, Thickness to, EasingMode easingMode)
    {
        var animation = new ThicknessAnimation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            EasingFunction = new QuadraticEase{ EasingMode = easingMode},
            To = to
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin(element);
    }

    private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Close(object sender, RoutedEventArgs e) => Close();

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        RadioButton1.IsChecked = true;
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var animation = new ThicknessAnimation
        {
            To = new Thickness(PageRbStackPanel.Children.IndexOf((UIElement)sender) * -750,0,0,0),
            Duration = TimeSpan.FromMilliseconds(300),
            EasingFunction = new QuadraticEase{ EasingMode = EasingMode.EaseInOut }
        };
        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, PagesStackPanel);
        Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));
        storyboard.Children.Add(animation);
        storyboard.Begin(PagesStackPanel);
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        Utils.DiscordRpc.client.Dispose();
    }
}