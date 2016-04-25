using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Compositing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LumiaImagingSDKPlayground
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartoonPage : Page, INotifyPropertyChanged
    {
        private CartoonEffect effect;

        public bool DoesEnhanceEdges
        {
            get
            {
                return effect.DistinctEdges;
            }
            set
            {
                if (value != effect.DistinctEdges)
                {
                    effect.DistinctEdges = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoesEnhanceEdges)));
                }
            }
        }

        public CartoonPage()
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
                var sourceParam = (IImageProvider)e.Parameter;
                ImageElement.Source = effect = new CartoonEffect(sourceParam);
            }
        }
    }
}
