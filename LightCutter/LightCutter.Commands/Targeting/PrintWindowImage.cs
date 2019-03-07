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


            [DllImport("user32.dll")]
            public static extern int GetSystemMetrics(SystemMetric smIndex);
            public enum SystemMetric : int
            {
                SM_CXSCREEN = 0,
                SM_CYSCREEN = 1,
                SM_CYVSCROLL = 2,
                SM_CXVSCROLL = 3,
                SM_CYCAPTION = 4,
                SM_CXBORDER = 5,
                SM_CYBORDER = 6,
                SM_CXDLGFRAME = 7,
                SM_CYDLGFRAME = 8,
                SM_CYVTHUMB = 9,
                SM_CXHTHUMB = 10,
                SM_CXICON = 11,
                SM_CYICON = 12,
                SM_CXCURSOR = 13,
                SM_CYCURSOR = 14,
                SM_CYMENU = 15,
                SM_CXFULLSCREEN = 16,
                SM_CYFULLSCREEN = 17,
                SM_CYKANJIWINDOW = 18,
                SM_MOUSEWHEELPRESENT = 75,
                SM_CYHSCROLL = 20,
                SM_CXHSCROLL = 21,
                SM_DEBUG = 22,
                SM_SWAPBUTTON = 23,
                SM_RESERVED1 = 24,
                SM_RESERVED2 = 25,
                SM_RESERVED3 = 26,
                SM_RESERVED4 = 27,
                SM_CXMIN = 28,
                SM_CYMIN = 29,
                SM_CXSIZE = 30,
                SM_CYSIZE = 31,
                SM_CXFRAME = 32,
                SM_CYFRAME = 33,
                SM_CXMINTRACK = 34,
                SM_CYMINTRACK = 35,
                SM_CXDOUBLECLK = 36,
                SM_CYDOUBLECLK = 37,
                SM_CXICONSPACING = 38,
                SM_CYICONSPACING = 39,
                SM_MENUDROPALIGNMENT = 40,
                SM_PENWINDOWS = 41,
                SM_DBCSENABLED = 42,
                SM_CMOUSEBUTTONS = 43,
                SM_CXFIXEDFRAME = SM_CXDLGFRAME,
                SM_CYFIXEDFRAME = SM_CYDLGFRAME,
                SM_CXSIZEFRAME = SM_CXFRAME,
                SM_CYSIZEFRAME = SM_CYFRAME,
                SM_SECURE = 44,
                SM_CXEDGE = 45,
                SM_CYEDGE = 46,
                SM_CXMINSPACING = 47,
                SM_CYMINSPACING = 48,
                SM_CXSMICON = 49,
                SM_CYSMICON = 50,
                SM_CYSMCAPTION = 51,
                SM_CXSMSIZE = 52,
                SM_CYSMSIZE = 53,
                SM_CXMENUSIZE = 54,
                SM_CYMENUSIZE = 55,
                SM_ARRANGE = 56,
                SM_CXMINIMIZED = 57,
                SM_CYMINIMIZED = 58,
                SM_CXMAXTRACK = 59,
                SM_CYMAXTRACK = 60,
                SM_CXMAXIMIZED = 61,
                SM_CYMAXIMIZED = 62,
                SM_NETWORK = 63,
                SM_CLEANBOOT = 67,
                SM_CXDRAG = 68,
                SM_CYDRAG = 69,
                SM_SHOWSOUNDS = 70,
                SM_CXMENUCHECK = 71,
                SM_CYMENUCHECK = 72,
                SM_SLOWMACHINE = 73,
                SM_MIDEASTENABLED = 74,
                SM_MOUSEPRESENT = 19,
                SM_XVIRTUALSCREEN = 76,
                SM_YVIRTUALSCREEN = 77,
                SM_CXVIRTUALSCREEN = 78,
                SM_CYVIRTUALSCREEN = 79,
                SM_CMONITORS = 80,
                SM_SAMEDISPLAYFORMAT = 81,
                SM_IMMENABLED = 82,
                SM_CXFOCUSBORDER = 83,
                SM_CYFOCUSBORDER = 84,
                SM_TABLETPC = 86,
                SM_MEDIACENTER = 87,
                SM_CMETRICS_OTHER = 76,
                SM_CMETRICS_2000 = 83,
                SM_CMETRICS_NT = 88,
                SM_REMOTESESSION = 0x1000,
                SM_SHUTTINGDOWN = 0x2000,
                SM_REMOTECONTROL = 0x2001,
            }

            #endregion

        } // end class

        public static Bitmap SaveByPrintWindowInside(string windowSelector)
        {
            // Find bounds of window
            var queryWindow = Desktop.Elements.Children(windowSelector);
            var window = (queryWindow.FirstOrDefault() as WpfElement)?.UIAutomationElement;
            var windowBounds = window?.Current.BoundingRectangle;

            System.Windows.Point toDevice = new System.Windows.Point(1, 1);

            var w = new System.Windows.Window {
                WindowStyle = System.Windows.WindowStyle.None,
                AllowsTransparency = true,
                Background = System.Windows.Media.Brushes.Transparent,
                Width = windowBounds.Value.Width,
                Height= windowBounds.Value.Height,
                Top = windowBounds.Value.Top,
                Left = windowBounds.Value.Left
            };
            try
            {
                w.Show();
                var source = System.Windows.PresentationSource.FromVisual(w);
                toDevice = new System.Windows.Point(
                    source.CompositionTarget.TransformToDevice.M11,
                    source.CompositionTarget.TransformToDevice.M22);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                w?.Close();
                w = null;
            }

            // TODO: Window Style
            double cxSizeFrame =
                NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXEDGE) +
                NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXBORDER) +
                (NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXSIZEFRAME)  + 1 ) * toDevice.X;

            double cySizeFrame = 
                NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYEDGE) +
                NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYBORDER) +
                (NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYSIZEFRAME) + 1) * toDevice.Y;


            double cyCaption = NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYCAPTION) * toDevice.Y;

            // TODO: option argument, for example : including VerticalScrollBar or else.
            double vscroll = NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXVSCROLL) * toDevice.X;

            System.Windows.Rect? bounds = new System.Windows.Rect(
                  windowBounds.Value.Left + cxSizeFrame,
                  windowBounds.Value.Top + cySizeFrame + cyCaption,
                  windowBounds.Value.Width - cxSizeFrame * 2 - vscroll,
                  windowBounds.Value.Height - cySizeFrame * 2 - cyCaption);

            if (bounds == null)
            {
                throw new TargetNotFoundException();
            } // end if

            return GetBitmapByUsingPrintWindow(window, windowBounds, bounds);
        } // end sub

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

            return GetBitmapByUsingPrintWindow(window, windowBounds, bounds);
        }

        private static Bitmap GetBitmapByUsingPrintWindow(System.Windows.Automation.AutomationElement window, System.Windows.Rect? windowBounds, System.Windows.Rect? bounds)
        {
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
