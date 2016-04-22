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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LumiaImagingSDKPlayground
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static StorageFile selectedFile;
        
        public MainPage()
        {
            this.InitializeComponent();
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

            selectedFile = await picker.PickSingleFileAsync();
            ImageElement.WorkingImage = new StorageFileImageSource(selectedFile);
        }
        
        private void Reset_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImageElement.WorkingImage = new StorageFileImageSource(selectedFile);
        }

        private void HDR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HDRPage), ImageElement.WorkingImage);
        }
    }
}
