using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public class ActionViewModel
    {
        public Visibility DefaultShortcutVisibility { get; set;  }

        public Visibility DefaultShortcutErrorVisibility { get; set; }

        public Visibility DefaultShortcutKeyVisibility { get; set; }

        public HotkeyViewModel DefaultShortcut { get; set; }
    }
}
