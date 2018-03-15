using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightCutter.UI.Sample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var k = HotkeyKey.F2;
            while (k != HotkeyKey.None)
            {

                this.hotkeys.Add(new Hotkey
                {
                    Caption = "Win + " + k,
                    Win = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            k = HotkeyKey.A;
            while (k != HotkeyKey.None)
            {

                switch (k)
                {
                    case HotkeyKey.C:
                    case HotkeyKey.M:
                    case HotkeyKey.N:
                    case HotkeyKey.P:
                    case HotkeyKey.S:
                    case HotkeyKey.T:
                    case HotkeyKey.W:
                        break;

                    default:
                        this.hotkeys.Add(new Hotkey
                        {
                            Caption = "Win + Shift + " + k,
                            Win = Visibility.Visible,
                            Shift = Visibility.Visible,
                            Key = k
                        });
                        break;
                }



                k = k.Next();
            }

            k = HotkeyKey.F1;
            while (k != HotkeyKey.None)
            {

                this.hotkeys.Add(new Hotkey
                {
                    Caption = "Ctrl + Shift + " + k,
                    Ctrl = Visibility.Visible,
                    Shift = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            k = HotkeyKey.A;
            while (k != HotkeyKey.None)
            {

                this.hotkeys.Add(new Hotkey
                {
                    Caption = "Ctrl + Shift + Alt + " + k,
                    Ctrl = Visibility.Visible,
                    Shift = Visibility.Visible,
                    Alt = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            this.shortcutBox.ItemsSource = this.hotkeys;
            //this.shortcutBox.SelectedIndex = 0;
            this.shortcutBox.Text = "Win + Shift + A";

            this.shortcutBox2.ItemsSource = this.hotkeys;
            this.shortcutBox2.Text = "Win + Shift + Z";
        }

        private List<Hotkey> hotkeys = new List<Hotkey>();

    }

    public class Hotkey
    {
        public HotkeyKey Key { get; set; }
        public string Caption { get; set; }

        public Visibility Win { get; set; } = Visibility.Collapsed;
        public Visibility Ctrl { get; set; } = Visibility.Collapsed;
        public Visibility Shift { get; set; } = Visibility.Collapsed;
        public Visibility Alt { get; set; } = Visibility.Collapsed;
    }

    public enum HotkeyKey
    {
        None = 0,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,

        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,

    }


    /// <summary>
    /// Static class which is defined extension methods.
    /// </summary>
    public static class HotkeyKeyExtensions
    {
        public static int ToInt(this HotkeyKey me)
        {
            return (int)me;
        } // end function

        public static string ToText(this HotkeyKey me)
        {
            return me.ToString();
        } // end function

        public static HotkeyKey Next(this HotkeyKey me)
        {
            if (me == HotkeyKey.Z) return HotkeyKey.F1;
            if (me == HotkeyKey.F16) return HotkeyKey.None;
            return (HotkeyKey)(((int)me) + 1);
        }

    } // end class

}
