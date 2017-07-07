using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCutterPrototype
{
    public class LightCutter
    {
        public static Image CutImage( Image image, Size size, float scale ) {

            var point = new PointF((image.Width - size.Width * scale) / 2, (image.Height - size.Height * scale) / 2);

            var newImage = new Bitmap(size.Width, size.Height);
            using(var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image,
                    new RectangleF(0, 0, size.Width, size.Height),
                    new RectangleF(point.X, point.Y, size.Width * scale, size.Height * scale), 
                    GraphicsUnit.Pixel);
            }
            return newImage;
        }
    }
}
