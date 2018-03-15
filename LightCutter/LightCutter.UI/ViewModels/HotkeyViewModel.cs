using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public class HotkeyViewModel
    {
        public HotKeys Key { get; set; }
        public string Caption { get; set; }

        public Visibility Win { get; set; } = Visibility.Collapsed;
        public Visibility Ctrl { get; set; } = Visibility.Collapsed;
        public Visibility Shift { get; set; } = Visibility.Collapsed;
        public Visibility Alt { get; set; } = Visibility.Collapsed;
    } // end class

    public enum HotKeys
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
    public static class HotKeysExtensions
    {
        public static int ToInt(this HotKeys me)
        {
            return (int)me;
        } // end function

        public static string ToText(this HotKeys me)
        {
            return me.ToString();
        } // end function

        public static HotKeys Next(this HotKeys me)
        {
            if (me == HotKeys.Z) return HotKeys.F1;
            if (me == HotKeys.F16) return HotKeys.None;
            return (HotKeys)(((int)me) + 1);
        }

    } // end class
} // end namespace
