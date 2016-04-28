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
using Windows.Storage;
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
    public sealed partial class DrawPage : Page, INotifyPropertyChanged
    {
        public DrawPage()
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

            MyInkCanvas.InkPresenter.IsInputEnabled = true;
            MyInkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Touch | CoreInputDeviceTypes.Pen;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IImageProvider)
            {
                ImageElement.Source = (IImageProvider)e.Parameter;
            }
        }

        private async void SaveScribbles_Click(object sender, RoutedEventArgs e)
        {
            var file = await KnownFolders.SavedPictures.CreateFileAsync("Scribbles.gif", CreationCollisionOption.GenerateUniqueName);
            using (var stream = (await file.OpenStreamForWriteAsync()).AsOutputStream())
            {
                await MyInkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
            }
        }

        private async void SaveBoth_Click(object sender, RoutedEventArgs e)
        {
            using (var scribblesStream = new MemoryStream())
            {
                var scribbleOutputStream = scribblesStream.AsOutputStream();
                await MyInkCanvas.InkPresenter.StrokeContainer.SaveAsync(scribbleOutputStream);

                scribblesStream.Position = 0;

                using (var scribblesSource = new StreamImageSource(scribblesStream))
                using (var scribblesBlender = new BlendEffect(ImageElement.Source, scribblesSource))
                using (var overlayRenderer = new JpegRenderer(scribblesBlender) { RenderOptions = RenderOptions.Cpu })
                {
                    var absoluteBoudningRect = MyInkCanvas.InkPresenter.StrokeContainer.BoundingRect;
                    var relativeBoundingRect = new Rect(absoluteBoudningRect.Left / MyInkCanvas.ActualWidth, absoluteBoudningRect.Top / MyInkCanvas.ActualHeight,
                                                        absoluteBoudningRect.Width / MyInkCanvas.ActualWidth, absoluteBoudningRect.Height / MyInkCanvas.ActualHeight);
                    scribblesBlender.TargetArea = relativeBoundingRect;

                    var buffer = await overlayRenderer.RenderAsync();

                    var file = await KnownFolders.SavedPictures.CreateFileAsync("ScribblesBoth.jpg", CreationCollisionOption.GenerateUniqueName);
                    
                    using (var outputStream = (await file.OpenStreamForWriteAsync()))
                    {
                        await buffer.AsStream().CopyToAsync(outputStream);
                    }
                }
            }
        }
    }
}
