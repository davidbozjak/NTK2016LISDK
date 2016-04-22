using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lumia.Imaging;
using Windows.Foundation;
using Windows.UI;

namespace LumiaImagingSDKPlayground
{
    static class DefaultImageProvider
    {
        public static IImageProvider DefaultImage { get; private set; }

        static DefaultImageProvider()
        {
            SetDefaultFileSource();
        }

        private static void SetDefaultFileSource()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = folder.GetFileAsync(@"Assets\DefaultImage.jpg").AsTask().Result;

            DefaultImage = new StorageFileImageSource(file);
        }
    }
}
