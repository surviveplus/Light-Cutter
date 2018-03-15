using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HotKeySample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.SetHotkey();
        }

        #region NativeMethods

        /// <summary>
        /// Allows managed code to call unmanaged functions with Platform Invocation Services (PInvoke).
        /// </summary>
        internal static class NativeMethods
        {
            #region Win32 API Definitions

            // Insert the code of Declare of DllImport. (see static code analysis CA1060)

            public const Int16 WS_EX_TOOLWINDOW = 0x80;
            public const Int16 GWL_EXSTYLE = -20;

            [DllImport("user32")] public extern static int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32")] public extern static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            // 'Public Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Long, ByVal hwnd As Long, ByVal Msg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
            [DllImport("user32")] public extern static IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

            // Public Const WM_HOTKEY = &H312
            public const Int32 WM_HOTKEY = 0x312;

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
            public const byte MOD_WIN = 0x8;

            // 'グローバルアトムテーブルに追加し識別値を取得
            // Public Declare Function GlobalAddAtom Lib "kernel32" Alias "GlobalAddAtomA" (ByVal lpString As String) As Integer
            [DllImport("kernel32")] public extern static IntPtr GlobalAddAtom(string lpString);

            // 'グローバルアトムテーブルの参照カウントを1つ減らす
            // Public Declare Function GlobalDeleteAtom Lib "kernel32" (ByVal nAtom As Integer) As Integer
            [DllImport("kernel32")] public extern static Int16 GlobalDeleteAtom(IntPtr nAtom);

            #endregion

        } // end class

        #endregion

        private IntPtr hotkeyIdA;
        private IntPtr hotkeyIdZ;
        private IntPtr hotkeyIdWinShiftA;

        public void SetHotkey()
        {
            if (this.hotkeyIdA != IntPtr.Zero)
            {
                NativeMethods.UnregisterHotKey(this.Handle, this.hotkeyIdA);
            }

            // KeyCode constant for HotKey:
            // https://msdn.microsoft.com/ja-jp/library/0z084th3(v=vs.90).aspx

            //        ' Ctrl + Shift + Alt + A
            this.hotkeyIdA = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7.A");
            var a = NativeMethods.RegisterHotKey(this.Handle, this.hotkeyIdA, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT, 65);
            this.ShortcutA = (a != 0);
            Debug.WriteLine("Ctrl + Shift + Alt + A : " + this.ShortcutA);

            //        ' Ctrl + Shift + Alt + Z
            this.hotkeyIdZ = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7.Z");
            var z = NativeMethods.RegisterHotKey(this.Handle, this.hotkeyIdZ, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT, 90);
            this.ShortcutZ = (z != 0);
            Debug.WriteLine("Ctrl + Shift + Alt + Z : " + this.ShortcutZ);

            //        ' Win + Shift + A
            this.hotkeyIdWinShiftA = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7.WinShiftA");
            var winV = NativeMethods.RegisterHotKey(this.Handle, this.hotkeyIdWinShiftA, NativeMethods.MOD_WIN | NativeMethods.MOD_SHIFT, 65);
            this.ShortcutWinShiftA = (winV != 0);
            Debug.WriteLine("Win + Shift  + A : " + this.ShortcutWinShiftA);

        }

        public bool ShortcutA { get; private set; }
        public bool ShortcutZ { get; private set; }
        public bool ShortcutWinShiftA { get; private set;  }

        protected override void WndProc(ref Message m)
        {

            var msg = m.Msg;
            var wParam = m.WParam;


            if (msg == NativeMethods.WM_HOTKEY && wParam == this.hotkeyIdA)
            {
                Debug.WriteLine("Ctrl + Shift + Alt + A");
                m.Result = IntPtr.Zero;
            }
            else if (msg == NativeMethods.WM_HOTKEY && wParam == this.hotkeyIdZ)
            {
                Debug.WriteLine("Ctrl + Shift + Alt + Z");
                m.Result = IntPtr.Zero;
            }
            else if (msg == NativeMethods.WM_HOTKEY && wParam == this.hotkeyIdWinShiftA)
            {
                Debug.WriteLine("Win + Shift + A");
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
                //return NativeMethods.CallWindowProc(IntPtr.Zero, hwnd, msg, wParam, lParam);
            }


        }
    }
}

