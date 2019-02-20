using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public class ActionViewModel : ViewModelBase
    {
        /// <summary>
        /// Backing field of DefaultShortcutVisibility property.
        /// </summary>
        private Visibility valueOfDefaultShortcutVisibility;

        /// <summary>
        /// Gets or sets the value of something.
        /// </summary>
        public Visibility DefaultShortcutVisibility
        {
            get
            {
                return this.valueOfDefaultShortcutVisibility;
            } // end get
            set
            {
                this.SetProperty( ref this.valueOfDefaultShortcutVisibility, value );
            } // end set
        } // end property


        public Visibility DefaultShortcutErrorVisibility { get; set; }

        public Visibility DefaultShortcutKeyVisibility { get; set; }

        public HotkeyViewModel DefaultShortcut { get; set; }

        public string AccessText { get; set; }
    }
}
