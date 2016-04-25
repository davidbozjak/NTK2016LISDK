using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System.ComponentModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LumiaImagingSDKPlayground
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HDRPage : Page, INotifyPropertyChanged
    {
        private HdrEffect effect;

        public double Strength
        {
            get
            {
                return effect.Strength;
            }
            set
            {
                if (value != effect.Strength)
                {
                    effect.Strength = MapToValidRange(value, 0, 1);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Strength)));
                }
            }
        }

        public double Gamma
        {
            get
            {
                return effect.Gamma;
            }
            set
            {
                if (value != effect.Gamma)
                {
                    effect.Gamma = MapToValidRange(value, 0.6, 1.4);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gamma)));
                }
            }
        }

        public double Saturation
        {
            get
            {
                return effect.Saturation;
            }
            set
            {
                if (value != effect.Saturation)
                {
                    effect.Saturation = MapToValidRange(value, 0.4, 1.6);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Saturation)));
                }
            }
        }

        public double NoiseSuppression
        {
            get
            {
                return effect.NoiseSuppression;
            }
            set
            {
                if (value != effect.NoiseSuppression)
                {
                    effect.NoiseSuppression = MapToValidRange(value, 0.1, 0.3);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoiseSuppression)));
                }
            }
        }
        
        public HDRPage()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (o, e) =>
            {
                Frame.Navigate(typeof(MainPage), ImageElement.Source);
            };

            this.PropertyChanged += (o, e) =>
            {
                ImageElement.Render();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IImageProvider)
            {
                var source = (IImageProvider)e.Parameter;
                ImageElement.Source = effect = new HdrEffect(source);
            }
        }
        
        private static double MapToValidRange(double value, double min, double max)
        {   
            // for mapping [A, B] -> [a, b] use (val - A)*(b-a)/(B-A) + a.
            // A = 0, B = 100, a = min, b = max

            return value * (max - min) / 100 + min;
        }
    }
}
