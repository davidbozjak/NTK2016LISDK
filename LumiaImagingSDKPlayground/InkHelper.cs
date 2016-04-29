using Lumia.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace LumiaImagingSDKPlayground
{
    static class InkHelper
    {
        public static async Task<Rect> GetDrawArea(IImageProvider source, InkCanvas canvas)
        {
            var image = source;

            Size imageSize;

            if (image is IImageResource)
            {
                imageSize = ((IImageResource)image).ImageSize;
            }
            else if (image is IAsyncImageResource)
            {
                imageSize = (await ((IAsyncImageResource)image).LoadAsync()).ImageSize;
            }
            else
            {
                using (var renderer = new BitmapRenderer(image))
                {
                    var tempBmp = await renderer.RenderAsync();
                    imageSize = tempBmp.Dimensions;
                }
            }

            return FitWithinWhilePreservingAspectRatio(new Rect(0, 0, imageSize.Width, imageSize.Height), new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight), true);
        }

        public static Rect FitWithinWhilePreservingAspectRatio(Rect area, Rect targetArea, bool roundToInt)
        {
            double areaAspectRatio = area.Width / area.Height;
            double targetAreaAspectRatio = targetArea.Width / targetArea.Height;

            double newWidth;
            double newHeight;
            double newX;
            double newY;

            if (areaAspectRatio < targetAreaAspectRatio)
            {
                newHeight = targetArea.Height;
                newWidth = newHeight * areaAspectRatio;

                newX = targetArea.X + ((targetArea.Width - newWidth) / 2);
                newY = targetArea.Y;
            }
            else
            {
                newWidth = targetArea.Width;
                newHeight = newWidth / areaAspectRatio;

                newX = targetArea.X;
                newY = targetArea.Y + ((targetArea.Height - newHeight) / 2);
            }

            if (roundToInt)
            {
                newX = Math.Round(newX);
                newY = Math.Round(newY);
                newWidth = Math.Round(newWidth);
                newHeight = Math.Round(newHeight);
            }

            return new Rect((float)newX, (float)newY, (float)newWidth, (float)newHeight);
        }
    }
}
