using Net.Surviveplus.LightCutter.Desktop.Pages;
using Net.Surviveplus.LightCutter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Net.Surviveplus.LightCutter.Desktop.BackgroundWindow;

namespace Net.Surviveplus.LightCutter.Desktop.Models
{
    public class HotkeyModel
    {
        public IntPtr Id { get; set; }
        public bool Enabled { get; set; }

        public int Modifiers { get; set; }
        public bool Win { get; set; } 
        public bool Ctrl { get; set; } 
        public bool Shift { get; set; }
        public bool Alt { get; set; } 

        public int Key { get; set; }
        public string Caption { get; set; }

        public static HotkeyModel FromString( string text) {

            var result = new HotkeyModel();

            var keys = 
                from k in text.Split('+')
                select k.Trim();

            result.Win = keys.Contains("Win");
            result.Ctrl = keys.Contains("Ctrl");
            result.Shift = keys.Contains("Shift");
            result.Alt = keys.Contains("Alt");

            if (result.Win) { result.Modifiers = result.Modifiers | NativeMethods.MOD_WIN; }
            if (result.Ctrl) { result.Modifiers = result.Modifiers | NativeMethods.MOD_CONTROL; }
            if (result.Shift) { result.Modifiers = result.Modifiers | NativeMethods.MOD_SHIFT; }
            if (result.Alt) { result.Modifiers = result.Modifiers | NativeMethods.MOD_ALT; }

            result.Key = (int)( Enum.Parse(typeof(HotKeys),  keys.LastOrDefault() ) );

            result.Caption = text;

            return result;
        }

        public HotkeyViewModel ToViewModel() {
            return new HotkeyViewModel
            {
                Alt = this.Alt.ToVisibleOrCollapsed(),
                Caption = this.Caption,
                Ctrl = this.Ctrl.ToVisibleOrCollapsed(),
                Shift = this.Shift.ToVisibleOrCollapsed(),
                Key = (HotKeys)this.Key,
                Win = this.Win.ToVisibleOrCollapsed()
            };
        }
    }
}
