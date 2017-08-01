using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Net.Surviveplus.CodedUIQuery;


namespace Net.Surviveplus.LightCutter
{
    public static class LightCutter
    {
        #region WinAPI

        private class WinAPI
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

            [System.Runtime.InteropServices.DllImport("User32.dll")]
            public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        } // end class

        #endregion

        public static void SaveByPrintWindow( string windowSelector, string screenSelector )
        {
            // Find bounds of window
            var queryWindow = Desktop.Elements.WaitForChildren(windowSelector, TimeSpan.FromSeconds(3));
            var window = (queryWindow.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var windowBounds = window?.Current.BoundingRectangle;

            // Find bounds of inside of window
            var queryScreen = queryWindow.Find(screenSelector);
            var ui = (queryScreen.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var bounds = ui?.Current.BoundingRectangle;
            //Debug.WriteLine("bounds:" + (bounds?.ToString() ?? "(null)") );

            if (bounds == null)
            {
                // TODO: throw new NotFoundAppException ?
                return;
            } // end if


            // get window image
            using (var windowBitmap = new Bitmap((int)windowBounds.Value.Width, (int)windowBounds.Value.Height))
            {
                using (var g = Graphics.FromImage(windowBitmap))
                {
                    WinAPI.PrintWindow(new IntPtr(window.Current.NativeWindowHandle), g.GetHdc(), 0);
                } // end using

                // filename for new image
                var bitmapFile = new System.IO.FileInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyyMMdd HHmmss") + ".png"));

                // Save image
                using (var bitmap = new Bitmap((int)bounds.Value.Width, (int)bounds.Value.Height))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(windowBitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle((int)(bounds.Value.Left - windowBounds.Value.Left), (int)(bounds.Value.Top - windowBounds.Value.Top), bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                    } // end using

                    bitmap.Save(bitmapFile.FullName, ImageFormat.Png);
                } // end using
            } // end using (windowBitmap)
        } // end sub

        public static void SaveByCopyFromScreen(string windowSelector, string screenSelector)
        {
            // Find bounds of window
            var queryWindow = Desktop.Elements.WaitForChildren(windowSelector, TimeSpan.FromSeconds(3));
            var window = (queryWindow.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var windowBounds = window?.Current.BoundingRectangle;

            // Find bounds of inside of window
            var queryScreen = queryWindow.Find(screenSelector);
            var ui = (queryScreen.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var bounds = ui?.Current.BoundingRectangle;
            //Debug.WriteLine("bounds:" + (bounds?.ToString() ?? "(null)") );

            if (bounds == null)
            {
                // TODO: throw new NotFoundAppException ?
                return;
            } // end if


            // filename for new image
            var bitmapFile = new System.IO.FileInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyyMMdd HHmmss") + ".png"));

            // Save image
            using (var bitmap = new Bitmap((int)bounds.Value.Width, (int)bounds.Value.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(bounds.Value.TopLeft.ToPoint(), new Point(0, 0), bounds.Value.Size.ToSize());
                } // end using

                bitmap.Save(bitmapFile.FullName, ImageFormat.Png);
            } // end using
        } // end sub

    public static async Task CutRemoteDesktopInsiderAsync()
        {
            await Task.Run(() => {
                SaveByPrintWindow("{TscShellContainerClass}", "{UIMainClass}");
            });
        } // end sub

        public static async Task CutPrimaryDisplay()
        {
            await Task.Run(() =>
            {
                var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                Debug.WriteLine("bounds:" + (bounds.ToString() ?? "(null)"));

                if (bounds == null)
                {
                    // TODO: throw new NotFoundAppException ?
                    return;
                } // end if

                // filename for new image
                var bitmapFile = new System.IO.FileInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), DateTime.Now.ToString("yyyyMMdd HHmmss") + ".png"));

                // Save image
                using (var bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(bounds.Location, new Point(0, 0), bounds.Size);
                    } // end using

                    bitmap.Save(bitmapFile.FullName, ImageFormat.Png);
                } // end using
            });
        }

        public static async Task CutVMConnectInsiderAsync()
        {
            await Task.Run(() => {
                // <vmconnect.exe>{WindowsForms10.Window.8.app.0.2bef119_r6_ad1}
                // HACK: Require Administrator !!
                SaveByPrintWindow("{WindowsForms10.Window.8.app.0.131a902_r6_ad1}", "{UIMainClass}");
            });
        } // end sub

        public static async Task CutChromeRemoteDesktopInsiderAsync()
        {
            await Task.Run(() => {
                //{Chrome_WidgetWin_1} {Chrome_RenderWidgetHostHWND}
                SaveByCopyFromScreen("{Chrome_WidgetWin_1}", "{Chrome_RenderWidgetHostHWND}");
            });
        } // end sub


        public static async Task CutIeInsiderAsync()
        {
            await Task.Run(() => {
                SaveByPrintWindow("{IEFrame}", "{Frame Tab}");
            });
        } // end sub
} // end class
} // end namespace
