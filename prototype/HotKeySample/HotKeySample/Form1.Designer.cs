using System;
using System.Collections.Generic;
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
                this.RemoveHotKey();
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

        private List<Hotkey> hotkeys = new List<Hotkey>();

        public void RemoveHotKey() {
            foreach (var h in this.hotkeys)
            {
                if (h.Enabled)
                {
                    NativeMethods.UnregisterHotKey(this.Handle, h.Id);
                }
            }

            this.hotkeys.Clear();
        }

        public void SetHotkey()
        {
            this.RemoveHotKey();

            // KeyCode constant for HotKey:
            // https://msdn.microsoft.com/ja-jp/library/0z084th3(v=vs.90).aspx

            this.hotkeys.Add(new Hotkey {
                Caption = "Ctrl + Shift + Alt + A",
                Modifiers = NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT ,
                Key = 65});

            this.hotkeys.Add(new Hotkey
            {
                Caption = "Ctrl + Shift + Alt + Z",
                Modifiers = NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_ALT,
                Key = 90
            });

            this.hotkeys.Add(new Hotkey
            {
                Caption = "Win + Shift + A",
                Modifiers = NativeMethods.MOD_WIN | NativeMethods.MOD_SHIFT,
                Key = 65
            });

            foreach (var h in this.hotkeys)
            {
                h.Id = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7.{" + h.Caption + "}");
                var result = NativeMethods.RegisterHotKey(this.Handle, h.Id, h.Modifiers, h.Key);
                h.Enabled = (result != 0);
                Debug.WriteLine( h.Caption + " : " + h.Enabled);
            }

        } // end sub

        public bool ShortcutWinShiftA { get; private set;  }

        protected override void WndProc(ref Message m)
        {
            var msg = m.Msg;
            var wParam = m.WParam;

            bool handled = false;
            foreach (var h in this.hotkeys)
            {
                if (msg == NativeMethods.WM_HOTKEY && wParam == h.Id)
                {
                    Debug.WriteLine(h.Caption);
                    m.Result = IntPtr.Zero;
                    handled = true;
                }
            }

            if(handled == false){
                base.WndProc(ref m);
                //return NativeMethods.CallWindowProc(IntPtr.Zero, hwnd, msg, wParam, lParam);
            }


        } // end sub

    } // end class


    public class Hotkey
    {
        public IntPtr Id { get; set; }
        public bool Enabled { get; set; }

        public int Modifiers { get; set; }
        public int Key { get; set; }
        public string Caption { get; set; }
    }

} // end namespace

