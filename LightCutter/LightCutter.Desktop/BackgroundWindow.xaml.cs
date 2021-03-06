﻿using Net.Surviveplus.LightCutter.Desktop.Models;
using Net.Surviveplus.LightCutter.Desktop.Properties;
using Net.Surviveplus.LightCutter.UI;
using Net.Surviveplus.LightCutter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            public const Int16 WS_EX_TOOLWINDOW = 0x80;
            public const Int16 GWL_EXSTYLE = -20;

            [DllImport("user32")] public extern static int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32")] public extern static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

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
            public const byte MOD_WIN = 0x8;

            // 'グローバルアトムテーブルに追加し識別値を取得
            // Public Declare Function GlobalAddAtom Lib "kernel32" Alias "GlobalAddAtomA" (ByVal lpString As String) As Integer
            [DllImport("kernel32")] public extern static IntPtr GlobalAddAtom(string lpString);

            // 'グローバルアトムテーブルの参照カウントを1つ減らす
            // Public Declare Function GlobalDeleteAtom Lib "kernel32" (ByVal nAtom As Integer) As Integer
            [DllImport("kernel32")] public extern static Int16 GlobalDeleteAtom(IntPtr nAtom);

            public const byte WM_SETTINGCHANGE = 0x001A;
            #endregion

        } // end class

        #endregion

        #region constructors

        public BackgroundWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.Upgraded == false)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeDefaultActionName();
                Properties.Settings.Default.Upgraded = true;
                Properties.Settings.Default.SaveAndUpdateCommandsSettings();
            }
            else
            {
                Properties.Settings.Default.UpdateCommandsSettings();
            }

            LightCutter.InitializeActionCommands();
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

        private List<HotkeyModel> hotkeys = new List<HotkeyModel>();
        public HotkeyModel ShortcutOpenActionPanel { get; private set; }
        public HotkeyViewModel ShortcutOpenActionPanelViewModel { get {
                return this.ShortcutOpenActionPanel.ToViewModel();
            } }

        public HotkeyModel ShortcutStartDefaultAction { get; private set; }
        public HotkeyViewModel ShortcutStartDefaultActionViewModel {
            get
            {
                return this.ShortcutStartDefaultAction.ToViewModel();
            }
        }

        private WindowInteropHelper helper;



        public void SetHotkey()
        {
            this.ReleaseHotkey();

            // KeyCode constant for HotKey:
            // https://msdn.microsoft.com/ja-jp/library/0z084th3(v=vs.90).aspx

            try
            {
                this.ShortcutOpenActionPanel = HotkeyModel.FromString(Settings.Default.ShortcutOpenActionPanel);
            }
            catch (Exception)
            {
                Settings.Default.ShortcutOpenActionPanel = "Ctrl + Shift + Alt + A";
                Settings.Default.SaveAndUpdateCommandsSettings();
                this.ShortcutOpenActionPanel = HotkeyModel.FromString(Settings.Default.ShortcutOpenActionPanel);
            }
            this.hotkeys.Add(this.ShortcutOpenActionPanel);

            try
            {
                this.ShortcutStartDefaultAction = HotkeyModel.FromString(Settings.Default.ShortcutStartDefaultAction);
            }
            catch (Exception)
            {
                Settings.Default.ShortcutStartDefaultAction = "Ctrl + Shift + Alt + Z";
                Settings.Default.SaveAndUpdateCommandsSettings();
                this.ShortcutStartDefaultAction = HotkeyModel.FromString(Settings.Default.ShortcutStartDefaultAction);
            }
            this.hotkeys.Add(this.ShortcutStartDefaultAction);

            foreach (var h in this.hotkeys)
            {
                h.Id = NativeMethods.GlobalAddAtom("Net.Surviveplus.LightCutter7.{" + h.Caption + "}");
                var result = NativeMethods.RegisterHotKey(this.helper.Handle, h.Id, h.Modifiers, h.Key);
                h.Enabled = (result != 0);
                Debug.WriteLine(h.Caption + " : " + h.Enabled);
            }
        }

        public void ReleaseHotkey()
        {
            foreach (var h in this.hotkeys)
            {
                if (h.Enabled)
                {
                    NativeMethods.UnregisterHotKey(this.helper.Handle, h.Id);
                    NativeMethods.GlobalDeleteAtom(h.Id);
                }
            }
            this.hotkeys.Clear();
        }

        #endregion

        #region Window events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.helper = new WindowInteropHelper(this);
            this.hwndSource = HwndSource.FromHwnd(this.helper.Handle);
            this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));

            var style = NativeMethods.GetWindowLong(this.helper.Handle, NativeMethods.GWL_EXSTYLE);
            style = style | NativeMethods.WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLong(this.helper.Handle, NativeMethods.GWL_EXSTYLE, style);

            this.SetHotkey();

            if(Properties.Settings.Default.ShowActionPanelOnStartUp){
                this.ShowActionPannel();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.main?.Close();
            this.ReleaseHotkey();
        }

        private HwndSource hwndSource;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == NativeMethods.WM_SETTINGCHANGE)
            {
                WindowsTheme.Current.Theme = WindowsTheme.GetUserTheme().Theme;
                handled = true;
                return IntPtr.Zero;
            }

            foreach (var h in this.hotkeys)
            {
                if (msg == NativeMethods.WM_HOTKEY && wParam == h.Id)
                {
                    if (h == this.ShortcutOpenActionPanel)
                    {
                        this.ShowActionPannel();
                    }
                    else if (h == this.ShortcutStartDefaultAction)
                    {
                        this.DoDefaultAction();
                    }
                    handled = true;
                    return IntPtr.Zero;
                }
            }
            return NativeMethods.CallWindowProc(IntPtr.Zero, hwnd, msg, wParam, lParam);
        }


        #endregion

        #region Notify menu events

        private  async void Count3CutAndSaveAction_Click(object sender, RoutedEventArgs e)
        {
            var time = DateTime.Now + TimeSpan.FromSeconds(3);
            await Task.Run(() => { System.Threading.Thread.Sleep(200); });

            using (new WindowHide(main))
            {
                var action = Commands.ActionCommands.FromCommands("Wait > Screen > Cut > Save");
                LightCutter.TryDo(() => action.Do(), ()=> action.ToString());
            }
        }

        private async void CutAndCopyAction_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { System.Threading.Thread.Sleep(200); });
            using (new WindowHide(main))
            {
                var action = Commands.ActionCommands.FromCommands("Screen > Cut > Copy");
                LightCutter.TryDo(() => action.Do(), () => action.ToString());
            }
        }

        private async void CutAndSaveAction_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { System.Threading.Thread.Sleep(200); });
            using (new WindowHide(main))
            {
                var action = Commands.ActionCommands.FromCommands("Screen > Cut > Save");
                LightCutter.TryDo(() => action.Do(), () => action.ToString());
            }
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
            this.DoDefaultAction();
        }

        #endregion

        private void DoDefaultAction()
        {
            if(LightCutter.ActionCommands.ContainsKey( Settings.Default.DefaultActionName))
            {
                var action = LightCutter.ActionCommands[Settings.Default.DefaultActionName];
                if (action.IsEnabled)
                {
                    using (new WindowHide(main))
                    {
                        LightCutter.TryDo(() => action.Do(), () => action.ToString());
                    }
                }
            }

        }

    } // end class
} // end namespace
