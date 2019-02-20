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
        /// Gets or sets whether DefaultShortcut is shown or not.
        /// </summary>
        public Visibility DefaultShortcutVisibility
        {
            get => this.valueOfDefaultShortcutVisibility; 
            set => this.SetProperty(ref this.valueOfDefaultShortcutVisibility, value); 
        } // end property

        public DefaultShortcutViewModel DefaultShortcut { get; set; } 

        public string AccessText { get; set; }

        public string Name { get; set; }
    }
}
