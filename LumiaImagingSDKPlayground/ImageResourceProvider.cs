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
    }
}
