using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;

namespace Kurai.Launcher.Pages;

public partial class NewsPage : Page
{
    public NewsPage()
    {
        InitializeComponent();
        
        for (var i = 0; i < 7; i++)
        {
            NewsPanel.Children.Add(
                new NewsItem
                {
                    Title = "Lorem Ipsum",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam blandit imperdiet mauris, sit amet gravida nisl mattis sed. Vivamus suscipit tortor eu justo pellentesque faucibus. Aenean in dolor nunc. Morbi nisl tortor, euismod non justo vitae, volutpat faucibus metus. Ut egestas efficitur euismod. Fusce nec ex ut elit mattis euismod a a lectus. Morbi ipsum risus, iaculis at ultricies non, mattis eu lacus. Nunc tortor est, auctor a porta convallis, dapibus vitae nunc. Morbi non rutrum lorem, vitae porta est. In id dapibus libero. Nunc libero leo, congue in iaculis eu, feugiat at sapien. Cras volutpat nunc lacus, ut dapibus tellus eleifend a. Ut eget turpis fringilla, eleifend mauris at, ornare ligula. Fusce et tempus sem. Pellentesque libero leo, mollis eu efficitur sed, porttitor ut sem.\n\nNullam nec sem ac ipsum ullamcorper tincidunt. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor ex ac tempor posuere. Quisque aliquam quam vel enim tristique commodo. In at placerat eros. Ut hendrerit lorem sed odio condimentum, ut tincidunt nisl lobortis. In ante ipsum, hendrerit id euismod pulvinar, accumsan ut lectus. Pellentesque laoreet feugiat lacus. Nulla sapien tellus, scelerisque in varius at, tincidunt sollicitudin ipsum. Etiam orci velit, sodales quis elit non, porta bibendum tortor.\n\nAliquam erat volutpat. Etiam sit amet iaculis mi. Nulla nisi felis, tempor quis lectus sit amet, commodo aliquam diam. Nunc vel tellus ut ex laoreet mollis et vitae metus. In hac habitasse platea dictumst. Donec quis nibh enim. Sed sem augue, ornare ut convallis eget, tempus at ipsum. Integer a interdum nulla. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nunc fermentum vestibulum ligula, at auctor nisi imperdiet eu. Sed ultrices condimentum urna vitae bibendum. Nam lobortis fermentum ullamcorper.\n\nUt luctus magna a nibh condimentum, quis sodales justo fringilla. In odio velit, congue a aliquet sit amet, viverra nec enim. Quisque tincidunt urna pretium, tristique nunc a, sodales est. Praesent ultricies leo augue, et fermentum urna ullamcorper ac. Duis augue ipsum, porttitor a metus vel, lobortis malesuada dui. Curabitur laoreet eu justo sit amet convallis. Donec tincidunt at est at pretium. Phasellus imperdiet interdum augue, auctor tempor sem ornare eu. Mauris dignissim non arcu non rutrum. Fusce cursus, orci non posuere dictum, sapien dui cursus diam, sed pellentesque libero dolor in nunc. Nunc metus ligula, tristique eu ultricies in, pulvinar sit amet mauris. Proin id dui sodales, fringilla odio sed, volutpat quam. Ut feugiat dolor eleifend dolor porttitor, vel sollicitudin ligula scelerisque.\n\nPraesent a viverra tellus. Nullam venenatis, ante facilisis efficitur aliquet, nulla nunc sodales justo, et suscipit dolor magna vitae velit. Mauris elementum urna vitae nulla hendrerit semper. Vivamus vehicula, ex vitae congue mollis, turpis erat posuere neque, a semper arcu nunc non dolor. Morbi laoreet, ligula faucibus sodales tincidunt, libero est aliquet massa, a fermentum lorem felis ut ipsum. Donec quis condimentum turpis. Pellentesque nunc felis, dapibus ut nunc eget, consequat pulvinar quam.",
                    Author = "happened again",
                    Date = "May 26th"
                }
            );
        }
    }
}
