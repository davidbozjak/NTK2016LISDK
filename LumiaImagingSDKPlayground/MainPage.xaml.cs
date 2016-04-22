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

using Lumia.Imaging;
using Windows.UI;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LumiaImagingSDKPlayground
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static IImageProvider originalSource;
        
        public MainPage()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IImageProvider)
            {
                ImageElement.WorkingImage = (IImageProvider)e.Parameter;
            }
            else
            {
                ImageElement.Render();
            }
        }

        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            splitPanel.IsPaneOpen = !splitPanel.IsPaneOpen;
        }

        private async void SelectFile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");

            var selectedFile = await picker.PickSingleFileAsync();
            originalSource = await CreateImageSourceFromFile(selectedFile);
            ImageElement.WorkingImage = originalSource;
        }
        
        private void Reset_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImageElement.WorkingImage = originalSource ?? DefaultImageProvider.DefaultImage;
        }

        private void HDR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HDRPage), ImageElement.WorkingImage);
        }

        private static async Task<IImageProvider> CreateImageSourceFromFile(StorageFile file)
        {
            //method needed, workaround for exif orientation bug

            using (var source = new StorageFileImageSource(file))
            using (var renderer = new BitmapRenderer(source) { RenderOptions = RenderOptions.Cpu })
            {
                var bitmap = await renderer.RenderAsync();
                return new BitmapImageSource(bitmap);
            }
        }
    }
}
