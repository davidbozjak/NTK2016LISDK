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
        public event Action RenderRequested;

        private StorageFile selectedFile;
        private SwapChainPanelRenderer renderer;
        private Task renderingTask;

        public IImageProvider WorkingImage
        {
            get { return renderer.Source; }
            set
            {
                if (value != renderer.Source)
                {
                    renderer.Source = value;
                    RenderRequested?.Invoke();
                }
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            renderer = new SwapChainPanelRenderer() { SwapChainPanel = mainDrawingArea };
            renderer.Source = new ColorImageSource(new Size(100, 100), Color.FromArgb(255, 255, 23, 123));
            
            RenderRequested += () =>
            {
                if (renderingTask?.IsCompleted ?? true)
                {
                    renderingTask = renderer.RenderAsync().AsTask();
                }
                else
                {
                    renderingTask = renderingTask.ContinueWith(async (_) => await renderer.RenderAsync());
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RenderRequested?.Invoke();
        }

        private async void btnSelectFile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");

            selectedFile = await picker.PickSingleFileAsync();
            WorkingImage = new StorageFileImageSource(selectedFile);
        }

        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            splitPanel.IsPaneOpen = !splitPanel.IsPaneOpen;
        }
    }
}
