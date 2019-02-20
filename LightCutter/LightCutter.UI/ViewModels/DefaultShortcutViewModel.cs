using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public class DefaultShortcutViewModel : ViewModelBase
    {

        public Visibility ErrorVisibility { get; set; }

        public Visibility HotkeyVisibility { get; set; }

        public HotkeyViewModel Hotkey { get; set; }
    }
}
