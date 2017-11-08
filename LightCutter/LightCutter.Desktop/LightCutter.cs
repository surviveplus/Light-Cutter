using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Desktop
{
    public class LightCutter
    {
        public static void CutAndCopy(MainWindow main, DateTime? time = null)
        {
            using (new WindowHide(main))
            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze(time))
            {
                var w = new UI.FullScreenWindow();
                var r = w.ShowFrozenScreen(frozen);
                if (r.HasValue && r.Value)
                {
                    using (var cropped = frozen?.Crop(w.CroppedBounds))
                    using (var bitmap = cropped?.GetBitmap())
                    {
                        System.Windows.Forms.Clipboard.SetImage(bitmap);
                    } // end using (cropped, bitmap )
                } // end if
            } // end using(frozen)

        }

        public static void CutAndSave(MainWindow main, DateTime? time = null)
        {
            using (new WindowHide(main))
            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze(time))
            {
                var w = new UI.FullScreenWindow();
                var r = w.ShowFrozenScreen(frozen);
                if (r.HasValue && r.Value)
                {
                    using (var cropped = frozen?.Crop(w.CroppedBounds))
                    using (var bitmap = cropped?.GetBitmap())
                    {

                        var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
                        bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
                    } // end using (cropped, bitmap )
                } // end if
            } // end using(frozen)
        }

    }
}
