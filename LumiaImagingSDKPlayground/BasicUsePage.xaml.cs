using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
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
    public sealed partial class BasicUsePage : Page, INotifyPropertyChanged
    {
        private IImageProvider source;
        private BlendEffect logoEffect;
        private BlendEffect bozjakHeadshotEffect;
        private BlurEffect blurEffect;
        
        private bool logoShown;
        public bool LogoShown
        {
            get { return logoShown; }
            set
            {
                if (value != logoShown)
                {
                    logoShown = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogoShown)));
                }
            }
        }

        private bool bozjakFaceShown;
        public bool BozjakFaceShown
        {
            get { return bozjakFaceShown; }
            set
            {
                if (value != bozjakFaceShown)
                {
                    bozjakFaceShown = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BozjakFaceShown)));
                }
            }
        }

        public bool BlurShown => BlurKernelSize > 0;

        private int blurKernelSize;
        public int BlurKernelSize
        {
            get { return blurKernelSize; }
            set
            {
                if (value != blurKernelSize)
                {
                    blurKernelSize = value;
                    if (blurKernelSize > 0)
                    {
                        blurEffect.KernelSize = blurKernelSize;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlurKernelSize)));
                }
            }
        }

        public BasicUsePage()
        {
            this.InitializeComponent();
            
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (o, e) =>
            {
                Frame.Navigate(typeof(MainPage), ImageElement.WorkingImage);
            };

            this.PropertyChanged += (o, e) =>
            {
                SetUpRenderingChain();
                ImageElement.Render();
            };

            logoEffect = new BlendEffect();
            logoEffect.ForegroundSource = ImageResourceProvider.NTKLogo;
            logoEffect.TargetArea = new Rect(0.9, 0.9, 0.1, 0.1);
            logoEffect.TargetOutputOption = OutputOption.PreserveAspectRatio;

            bozjakHeadshotEffect = new BlendEffect();
            bozjakHeadshotEffect.ForegroundSource = ImageResourceProvider.BozjakHeadshot;
            bozjakHeadshotEffect.TargetArea = new Rect(0.05, 0.05, 0.2, 0.2);
            bozjakHeadshotEffect.TargetOutputOption = OutputOption.PreserveAspectRatio;

            blurEffect = new BlurEffect();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IImageProvider)
            {
                var sourceParam = (IImageProvider)e.Parameter;
                ImageElement.WorkingImage = source = sourceParam;
            }
        }

        private void SetUpRenderingChain()
        {
            IImageProvider finalEmenent = source;

            if (BlurShown)
            {
                blurEffect.Source = finalEmenent;
                finalEmenent = blurEffect;
            }

            if (LogoShown)
            {
                logoEffect.Source = finalEmenent;
                finalEmenent = logoEffect;
            }

            if (BozjakFaceShown)
            {
                bozjakHeadshotEffect.Source = finalEmenent;
                finalEmenent = bozjakHeadshotEffect;
            }

            ImageElement.WorkingImage = finalEmenent;
        }
    }
}
