using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Net.Surviveplus.LightCutter.UI.Parts
{
    /// <summary>
    /// Uac.xaml の相互作用ロジック
    /// </summary>
    public partial class Uac : UserControl
    {
        /// <summary>
        /// Allows managed code to call unmanaged functions with Platform Invocation Services (PInvoke).
        /// </summary>
        internal static class NativeMethods
        {
            #region Win32 API Definitions

            // Insert the code of Declare of DllImport. (see static code analysis CA1060)
            public const int MAX_PATH = 260;
            public const uint SIID_SHIELD = 0x00000004D;
            public const uint SHGSI_ICON = 0x000000100;
            public const uint SHGSI_LARGEICON = 0x000000000;
            public const uint SHGSI_SMALLICON = 0x000000001;

            [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct SHSTOCKICONINFO
            {
                public uint cbSize;
                public IntPtr hIcon;
                public int iSysIconIndex;
                public int iIcon;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
                public string szPath;
            }

            [DllImport("shell32.dll", SetLastError = false)]
            public static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO sii);

            #endregion

        } // end class

        public static Icon GetShieldIcon(bool smallSize)
        {
            //Windows Vista or later
            if (Environment.OSVersion.Platform != PlatformID.Win32NT ||
                Environment.OSVersion.Version.Major < 6)
            {
                return null;
            }

            var sii = new NativeMethods.SHSTOCKICONINFO();
            sii.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.SHSTOCKICONINFO));
            int res = NativeMethods.SHGetStockIconInfo(NativeMethods.SIID_SHIELD, NativeMethods.SHGSI_ICON | (smallSize ? NativeMethods.SHGSI_SMALLICON : NativeMethods.SHGSI_LARGEICON), ref sii);

            if (res != 0)
            {
                Marshal.ThrowExceptionForHR(res);
            }

            //Iconオブジェクトを作成する
            return Icon.FromHandle(sii.hIcon);
        }

        public Uac()
        {
            InitializeComponent();

            using (var icon = GetShieldIcon(true))
            {
                this.uacIcon.Width = icon.Width;
                this.uacIcon.Height = icon.Height;
                this.uacIcon.Source =  Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
