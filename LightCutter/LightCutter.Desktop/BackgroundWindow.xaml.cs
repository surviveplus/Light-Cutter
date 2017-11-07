﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Net.Surviveplus.LightCutter.Desktop
{
    /// <summary>
    /// BackgroundWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BackgroundWindow : Window
    {
        #region NativeMethods

        /// <summary>
        /// Allows managed code to call unmanaged functions with Platform Invocation Services (PInvoke).
        /// </summary>
        internal static class NativeMethods
        {
            #region Win32 API Definitions

            // Insert the code of Declare of DllImport. (see static code analysis CA1060)


            // 'Public Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Long, ByVal hwnd As Long, ByVal Msg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
            [DllImport("user32")] public extern static IntPtr CallWindowProc( IntPtr lpPrevWndFunc, IntPtr hwnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

            // Public Const WM_HOTKEY = &H312
            public const Int32  WM_HOTKEY = 0x312;

            // 'ホットキーを登録する
            // Public Declare Function RegisterHotKey Lib "user32.dll" (ByVal hWnd As Long, ByVal id As Long, ByVal fsModifiers As Long, ByVal vk As Long) As Long
            [DllImport("user32.dll")] public extern static Int32 RegisterHotKey(IntPtr hWnd, IntPtr id, Int32 fsModifiers, Int32 vk);

            // 'ホットキーを解除する
            // Public Declare Function UnregisterHotKey Lib "user32.dll" (ByVal hWnd As Long, ByVal id As Long) As Long
            [DllImport("user32.dll")] public extern static Int32 UnregisterHotKey(IntPtr hWnd, IntPtr id);

            // Public Const MOD_ALT = &H1             '[Alt]キー
            public const byte MOD_ALT = 0x1;

            // Public Const MOD_CONTROL = &H2         '[Ctrl]キー
            public const byte MOD_CONTROL = 0x2;

            // Public Const MOD_SHIFT = &H4           '[Shift]キー
            public const byte MOD_SHIFT = 0x4;

            // Public Const MOD_WIN = &H8             '[Windows]キー

            // 'グローバルアトムテーブルに追加し識別値を取得
            // Public Declare Function GlobalAddAtom Lib "kernel32" Alias "GlobalAddAtomA" (ByVal lpString As String) As Integer
            [DllImport("kernel32")] public extern static IntPtr GlobalAddAtom(string lpString);

            // 'グローバルアトムテーブルの参照カウントを1つ減らす
            // Public Declare Function GlobalDeleteAtom Lib "kernel32" (ByVal nAtom As Integer) As Integer
            [DllImport("kernel32")] public extern static Int16 GlobalDeleteAtom(IntPtr nAtom);

            #endregion

        } // end class

        #endregion

        #region constructors

        public BackgroundWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region methods

        private MainWindow main = null;
        
        public void ShowActionPannel()
        {
            if (this.main == null)
            {
                this.main = new MainWindow();
                this.main.Closed += (sender, e) => { this.main = null; };
            }
            this.main?.Show();
            if(this.main?.WindowState == WindowState.Minimized)
            {
                this.main.WindowState = WindowState.Normal;
            }
            this.main?.Activate();
        }

        private IntPtr hotkeyId ;
        private WindowInteropHelper helper;

        public void SetHotkey()
        {
            if (this.hotkeyId != IntPtr.Zero)
            {
                NativeMethods.UnregisterHotKey(this.helper.Handle, this.hotkeyId);
            }

            this.hotkeyId = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7");

            // https://msdn.microsoft.com/ja-jp/library/0z084th3(v=vs.90).aspx

            //        ' Ctrl + Shift + Alt + S
            //NativeMethods.RegisterHotKey(this.helper.Handle, this.hotkeyId, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT, 83);

            //        ' Ctrl + Shift + Alt + A
            NativeMethods.RegisterHotKey(this.helper.Handle, this.hotkeyId, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT, 65);
        }

        public void ReleaseHotkey()
        {
            if( this.hotkeyId != IntPtr.Zero)
            {
                NativeMethods.UnregisterHotKey(this.helper.Handle, this.hotkeyId);
                NativeMethods.GlobalDeleteAtom(this.hotkeyId);

                this.hotkeyId = IntPtr.Zero;
            }
        }

        #endregion

        #region Window events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.helper = new WindowInteropHelper(this);
            this.hwndSource = HwndSource.FromHwnd(this.helper.Handle);
            this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));

            this.SetHotkey();
            this.ShowActionPannel();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.main?.Close();
            this.ReleaseHotkey();
        }

        private HwndSource hwndSource;

        private IntPtr WndProc(IntPtr hwnd, int msg,
  IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_HOTKEY && wParam == this.hotkeyId)
            {
                LightCutter.CutAndSave(this.main);
                handled = true;
                return IntPtr.Zero;
            }
            else
            {
                return NativeMethods.CallWindowProc(IntPtr.Zero, hwnd, msg, wParam, lParam);
            }

        }
        #endregion

        #region Notify menu events
        private void CutAndCopyAction_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndCopy(this.main);
        }

        private void CutAndSaveAction_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.main);
        }


        private void OpenMainAction_Click(object sender, RoutedEventArgs e)
        {
            this.ShowActionPannel();
        }

        private void CloseAction_Click(object sender, RoutedEventArgs e)
        {
            this.Notify.Visibility = Visibility.Hidden;
            this.Close();
        }

        #endregion

        #region Notify Icon events

        private void Notify_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.main);
        }

        #endregion

    } // end class
} // end namespace