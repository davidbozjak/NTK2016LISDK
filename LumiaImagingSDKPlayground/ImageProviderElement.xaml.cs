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
using System.Threading.Tasks;

using Lumia.Imaging;
using Windows.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LumiaImagingSDKPlayground
{
    public sealed partial class ImageProviderElement : UserControl
    {
        private SwapChainPanelRenderer renderer;
        private Task renderingTask;

        public IImageProvider Source
        {
            get { return renderer.Source; }
            set
            {
                if (value != renderer.Source)
                {
                    renderer.Source = value;
                    Render();
                }
            }
        }

        public ImageProviderElement()
        {
            this.InitializeComponent();

            renderer = new SwapChainPanelRenderer();
            renderer.Source = ImageResourceProvider.DefaultImage;
        }
        
        public void Render()
        {
            if (renderer.SwapChainPanel == null)
            {
                return;
            }

            if (renderingTask?.IsCompleted ?? true)
            {
                renderingTask = renderer.RenderAsync().AsTask();
            }
            else
            {
                renderingTask = renderingTask.ContinueWith(async (_) => await renderer.RenderAsync());
            }
        }
        
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderingSurface.Width = e.NewSize.Width;
            renderingSurface.Height = e.NewSize.Height;

            if (renderer.SwapChainPanel == null)
            {
                renderer.SwapChainPanel = renderingSurface;
            }

            Render();
        }
    }
}
