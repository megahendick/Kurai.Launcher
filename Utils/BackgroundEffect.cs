using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.Numerics;
using System.Windows.Threading;
using System.Diagnostics;

namespace Kurai.Launcher
{
    public class BackgroundEffect : ShaderEffect
    {
        private static Uri MakePackUri(string relativeFile)
        {
            Assembly a = typeof(BackgroundEffect).Assembly;

            // Extract the short name.
            string assemblyShortName = a.ToString().Split(',')[0];

            string uriString = "pack://application:,,,/" +
                assemblyShortName +
                ";component/" +
                relativeFile;

            return new Uri(uriString, UriKind.Absolute);
        }

        private static PixelShader _pixelShader =
            new PixelShader() { UriSource = MakePackUri("/Assets/BackgroundEffect.ps") };

        public BackgroundEffect()
        {
            this.PixelShader = _pixelShader;
            //UpdateShaderValue(InputProperty);
            UpdateShaderValue(TimeProperty);
            UpdateShaderValue(ResolutionProperty);
        }

        /*public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BackgroundEffect), 0);*/

        public double Time
        {
            get { return (double)GetValue(TimeProperty); }
            set 
            { 
                SetValue(TimeProperty, value); 
                UpdateShaderValue(TimeProperty);
            }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(double), typeof(BackgroundEffect),
              new UIPropertyMetadata(0.0d, PixelShaderConstantCallback(0)));

        public Size Resolution
        {
            get { return (Size)GetValue(ResolutionProperty); }
            set { SetValue(ResolutionProperty, value); }
        }

        public static readonly DependencyProperty ResolutionProperty =
            DependencyProperty.Register("Resolution", typeof(Size), typeof(BackgroundEffect),
              new UIPropertyMetadata(new Size(100.0d, 100.0d), PixelShaderConstantCallback(0)));
    }
}
