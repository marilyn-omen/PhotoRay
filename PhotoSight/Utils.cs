using System.IO;
using System.Windows.Media.Imaging;

namespace PhotoSight
{
    public static class Utils
    {
        public static byte[] StreamToByteArray(Stream photoStream)
        {
            var image = new BitmapImage();
            image.SetSource(photoStream);
            using (var ms = new MemoryStream())
            {
                var btmMap = new WriteableBitmap(image);
                btmMap.SaveJpeg(ms, image.PixelWidth, image.PixelHeight, 0, 100);
                return ms.ToArray();
            }
        }
    }
}
