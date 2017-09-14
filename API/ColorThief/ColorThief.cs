using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColorThiefDotNet
{
    public partial class ColorThief
    {
        public const int DefaultColorCount = 5;
        public const int DefaultQuality = 10;
        public const bool DefaultIgnoreMinMax = true;
        public const int ColorDepth = 4;

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="pixelArray">Pixel array.</param>
        /// <param name="colorCount">The color count.</param>
        /// <returns></returns>
        private CMap GetColorMap(byte[][] pixelArray, int colorCount)
        {
            // Send array to quantize function which clusters values using median
            // cut algorithm

            var cmap = Mmcq.Quantize(pixelArray, colorCount);
            return cmap;
        }

        private byte[][] ConvertPixels(byte[] pixels, int pixelCount, int quality, bool ignoreMinMax)
        {


            var expectedDataLength = pixelCount * ColorDepth;
            if (expectedDataLength != pixels.Length)
            {
                throw new ArgumentException("(expectedDataLength = "
                                            + expectedDataLength + ") != (pixels.length = "
                                            + pixels.Length + ")");
            }

            // Store the RGB values in an array format suitable for quantize
            // function

            // numRegardedPixels must be rounded up to avoid an
            // ArrayIndexOutOfBoundsException if all pixels are good.

            var numRegardedPixels = (pixelCount + quality - 1) / quality;

            var numUsedPixels = 0;
            var pixelArray = new byte[numRegardedPixels][];

            for (var i = 0; i < pixelCount; i += quality)
            {
                var offset = i * ColorDepth;
                var b = pixels[offset];
                var g = pixels[offset + 1];
                var r = pixels[offset + 2];
                var a = pixels[offset + 3];

                
                if (a >= 125)
                {
                    // If pixel is mostly opaque and not white or black
                    if (!ignoreMinMax || (r+g+b > 15 && r+g+b < 750))
                    {
                        pixelArray[numUsedPixels] = new[] { r, g, b };
                        numUsedPixels++;
                    }
                }
            }

            // Remove unused pixels from the array
            var copy = new byte[numUsedPixels][];
            Array.Copy(pixelArray, copy, numUsedPixels);
            return copy;
        }

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors and return the base color from the largest cluster.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreMinMax">if set to <c>true</c> [ignore  white & black].</param>
        /// <returns></returns>
        public QuantizedColor GetColor(Bitmap sourceImage, int quality = DefaultQuality, bool ignoreMixMax = DefaultIgnoreMinMax)
        {
            var palette = GetPalette(sourceImage, DefaultColorCount, quality, ignoreMixMax);
            
            //var dominantColor = new QuantizedColor(new Color
            //{
            //    A = Convert.ToByte(palette.Average(a => a.Color.A)),
            //    R = Convert.ToByte(palette.Average(a => a.Color.R)),
            //    G = Convert.ToByte(palette.Average(a => a.Color.G)),
            //    B = Convert.ToByte(palette.Average(a => a.Color.B))
            //}, Convert.ToInt32(palette.Average(a => a.Population)));

            return palette[0];
        }

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreMinMax">if set to <c>true</c> [ignore white & black].</param>
        /// <returns></returns>
        /// <code>true</code>
        public List<QuantizedColor> GetPalette(Bitmap sourceImage, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreMinMax = DefaultIgnoreMinMax)
        {
            var pixelArray = GetPixelsFast(sourceImage, quality, ignoreMinMax);
            var cmap = GetColorMap(pixelArray, colorCount);
            if(cmap != null)
            {
                var colors = cmap.GeneratePalette();
                return colors;
            }
            return new List<QuantizedColor>();
        }

        private byte[][] GetPixelsFast(Bitmap sourceImage, int quality, bool ignoreMinMax)
        {
            if(quality < 1)
            {
                quality = DefaultQuality;
            }

            var pixels = GetIntFromPixel(sourceImage);
            var pixelCount = sourceImage.Width*sourceImage.Height;

            return ConvertPixels(pixels, pixelCount, quality, ignoreMinMax);
        }

        private byte[] GetIntFromPixel(Bitmap bmp)
        {
            var pixelList = new byte[bmp.Width * bmp.Height * 4];
            int count = 0;

            for (var x = 0; x < bmp.Width; x++)
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    var clr = bmp.GetPixel(x, y);

                    pixelList[count] = clr.B;
                    count++;

                    pixelList[count] = clr.G;
                    count++;

                    pixelList[count] = clr.R;
                    count++;

                    pixelList[count] = clr.A;
                    count++;
                }
            }

            return pixelList;
        }
    }
}
