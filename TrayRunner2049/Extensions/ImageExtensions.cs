using System.Drawing.Imaging;

namespace TrayRunner2049.Extensions;

/// <summary>
/// Extension methods for Image and Icon data types.
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Converts an Image to an Icon with a specified size.
    /// </summary>
    /// <param name="image">Source Image to convert</param>
    /// <param name="size">Icon size for both width and height in pixels (max value 256)</param>
    /// <returns>Icon in the desired size</returns>
    /// <exception cref="ArgumentNullException">Thrown when image is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when size is less than or equal to 0 or greater than 256</exception>
    /// <exception cref="OutOfMemoryException">Thrown when there's insufficient memory to create the bitmap or icon</exception>
    public static Icon ToIcon(this Image image, int size = 32)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));

        using (var bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb))
        {
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawImage(image, new Rectangle(0, 0, size, size));

                return CreateIconFromBitmap(bmp);
            }
        }
    }

    /// <summary>
    /// Resizes an Image to the specified width and height.
    /// The operation produces high-quality results by using bicubic interpolation, antialiasing, and high-quality pixel offset mode.
    /// </summary>
    /// <param name="image">Source image to resize</param>
    /// <param name="width">Target width in pixels</param>
    /// <param name="height">Target height in pixels</param>
    /// <returns>A new resized Image with the specified dimensions</returns>
    /// <exception cref="ArgumentNullException">Thrown when image is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when width or height is less than or equal to 0</exception>
    /// <exception cref="OutOfMemoryException">Thrown when there's insufficient memory to create the resized image</exception>
    public static Image Resize(this Image image, int width, int height)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));

        Rectangle destRect = new Rectangle(0, 0, width, height);
        Bitmap destImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }
        return destImage;
    }

    /// <summary>
    /// Creates an Icon from a Bitmap by converting it to ICO format.
    /// This method constructs a proper ICO file structure with PNG data embedded inside.
    /// </summary>
    /// <param name="bitmap">The Bitmap to convert to an Icon</param>
    /// <returns>An Icon object created from the bitmap data</returns>
    /// <exception cref="ArgumentNullException">Thrown when bitmap is null</exception>
    /// <exception cref="ArgumentException">Thrown when bitmap has invalid dimensions</exception>
    /// <exception cref="OutOfMemoryException">Thrown when there's insufficient memory to create the icon</exception>
    private static Icon CreateIconFromBitmap(Bitmap bitmap)
    {
        using (var ms = new MemoryStream())
        {
            // ICO header
            ms.Write(BitConverter.GetBytes((short)0), 0, 2); // Reserved
            ms.Write(BitConverter.GetBytes((short)1), 0, 2); // ICO type
            ms.Write(BitConverter.GetBytes((short)1), 0, 2); // Number of images

            // Write the image entry
            byte width = (byte)(bitmap.Width >= 256 ? 0 : bitmap.Width);
            byte height = (byte)(bitmap.Height >= 256 ? 0 : bitmap.Height);
            ms.WriteByte(width); // width
            ms.WriteByte(height); // height
            ms.WriteByte(0); // color palette (0 = no palette)
            ms.WriteByte(0); // reserved
            ms.Write(BitConverter.GetBytes((short)0), 0, 2); // color planes
            ms.Write(BitConverter.GetBytes((short)32), 0, 2); // bits per pixel

            using (var imgStream = new MemoryStream())
            {
                // Save the image to PNG
                bitmap.Save(imgStream, ImageFormat.Png);
                byte[] pngData = imgStream.ToArray();

                ms.Write(BitConverter.GetBytes(pngData.Length), 0, 4); // size of image data
                ms.Write(BitConverter.GetBytes(22), 0, 4); // offset of image data
                ms.Write(pngData, 0, pngData.Length); // image data
            }

            ms.Position = 0;
            return new Icon(ms);
        }
    }
}