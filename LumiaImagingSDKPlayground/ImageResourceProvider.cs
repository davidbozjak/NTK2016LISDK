using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

using Lumia.Imaging;
using Windows.Foundation;
using Windows.UI;

namespace LumiaImagingSDKPlayground
{
    static class ImageResourceProvider
    {
        public static IImageProvider DefaultImage { get; private set; }

        public static IImageProvider NTKLogo { get; private set; }

        public static IImageProvider BozjakHeadshot { get; private set; }

        private static StorageFolder ResourceFolder
        {
            get
            {
                return Windows.ApplicationModel.Package.Current.InstalledLocation;
            }
        }

        static ImageResourceProvider()
        {
            LoadResources();
        }

        private static void LoadResources()
        {
            var defaultImageFile = ResourceFolder.GetFileAsync(@"Assets\DefaultImage.jpg").AsTask().Result;
            DefaultImage = new StorageFileImageSource(defaultImageFile);

            var ntkLogoFile = ResourceFolder.GetFileAsync(@"Assets\NTKLogo.png").AsTask().Result;
            NTKLogo = new StorageFileImageSource(ntkLogoFile);

            var bozjakHeadshotFile = ResourceFolder.GetFileAsync(@"Assets\Bozjak_HeadShot_2015.png").AsTask().Result;
            BozjakHeadshot = new StorageFileImageSource(bozjakHeadshotFile);
        }
        
        public static async Task<IImageProvider> CreateImageSourceFromFile(StorageFile file)
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
