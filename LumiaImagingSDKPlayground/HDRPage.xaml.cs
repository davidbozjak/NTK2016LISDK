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
                    effect.Strength = Math.Min(Math.Max(value / 100, 0), 1);
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
                    effect.Gamma = ClampToValidRange(value);
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
                    effect.Saturation = ClampToValidRange(value);
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
                    effect.NoiseSuppression = ClampToValidRange(value);
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
                Frame.Navigate(typeof(MainPage), effect);
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
                ImageElement.WorkingImage = effect = new HdrEffect(source);
            }
        }
        
        private static double ClampToValidRange(double value)
        {
            return Math.Min(Math.Max(value / 100, 1e-3), 1);
        }
    }
}
