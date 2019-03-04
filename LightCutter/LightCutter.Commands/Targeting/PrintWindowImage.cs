using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Net.Surviveplus.CodedUIQuery;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class PrintWindowImage
    {

        /// <summary>
        /// Allows managed code to call unmanaged functions with Platform Invocation Services (PInvoke).
        /// </summary>
        internal static class NativeMethods
        {
            #region Win32 API Definitions

            // Insert the code of Declare of DllImport. (see static code analysis CA1060)

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

            [System.Runtime.InteropServices.DllImport("User32.dll")]
            public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

            #endregion

        } // end class

        public static Bitmap PrintWindow(string windowSelector, string screenSelector)
        {

            // Find bounds of window
            var queryWindow = Desktop.Elements.Children(windowSelector);
            var window = (queryWindow.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var windowBounds = window?.Current.BoundingRectangle;

            // Find bounds of inside of window
            var queryScreen = queryWindow.Find(screenSelector);
            var ui = (queryScreen.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var bounds = ui?.Current.BoundingRectangle;
            //Debug.WriteLine("bounds:" + (bounds?.ToString() ?? "(null)") );

            if (bounds == null)
            {
                throw new TargetNotFoundException();
            } // end if


            // get window image
            using (var windowBitmap = new Bitmap((int)windowBounds.Value.Width, (int)windowBounds.Value.Height))
            {
                using (var g = Graphics.FromImage(windowBitmap))
                {
                    NativeMethods.PrintWindow(new IntPtr(window.Current.NativeWindowHandle), g.GetHdc(), 0);
                } // end using (g)

                var bitmap = new Bitmap((int)bounds.Value.Width, (int)bounds.Value.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(windowBitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle((int)(bounds.Value.Left - windowBounds.Value.Left), (int)(bounds.Value.Top - windowBounds.Value.Top), bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                } // end using (g)
                return bitmap;
            } // end using (windowBitmap)
        }
    }
}
