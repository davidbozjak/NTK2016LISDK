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
    public sealed partial class SketchPage : Page, INotifyPropertyChanged
    {
        private SketchEffect effect;

        public bool IsColor
        {
            get
            {
                return effect.SketchMode == SketchMode.Color;
            }
            set
            {
                SketchMode mode = value ? SketchMode.Color : SketchMode.Gray;

                if (mode != effect.SketchMode)
                {
                    effect.SketchMode = mode;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsColor)));
                }
            }
        }

        public SketchPage()
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
                ImageElement.Source = effect = new SketchEffect(sourceParam);
            }
        }
    }
}
