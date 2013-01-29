using System.IO;

namespace PhotoRay
{
    public static class Utils
    {
        public static byte[] StreamToByteArray(Stream photoStream)
        {
            using (var ms = new MemoryStream())
            {
                photoStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
