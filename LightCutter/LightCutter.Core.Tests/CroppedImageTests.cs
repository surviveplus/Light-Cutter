using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.LightCutter.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Core.Tests
{
    [TestClass()]
    public class CroppedImageTests
    {
        [TestMethod()]
        public void GetBitmap_Is_not_same_object_But_same_image()
        {
            var original = new System.Drawing.Bitmap(10, 10);
            var expectedColor = Color.AliceBlue;
            using (var g = System.Drawing.Graphics.FromImage(original)){
                g.FillRectangle(new SolidBrush(expectedColor), new Rectangle(0, 0, 10, 10));
            }
            
            using (var cropped = new CroppedImage(original))
            {
                var actual = cropped.GetBitmap();
                Assert.AreNotEqual(original, actual, "They are same object.");

                Assert.AreEqual(original.Width, actual.Width, "Width are not same.");
                Assert.AreEqual(original.Height, actual.Height, "Height are not same.");

                var actualColor = actual.GetPixel(9, 9);
                Assert.AreEqual(expectedColor.ToArgb(), actualColor.ToArgb(), "They are not same image (Pixel color).");               
            }
        }
    }
}