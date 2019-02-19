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

namespace Net.Surviveplus.LightCutter.UI.Parts
{
    /// <summary>
    /// ActionButton.xaml の相互作用ロジック
    /// </summary>
    public partial class ActionButton : UserControl
    {
        public ActionButton()
        {
            InitializeComponent();
        }

        ///// <summary>
        ///// Backing field of DefaultShortcutIsEnalbed property.
        ///// </summary>
        //private bool valueOfDefaultShortcutIsEnalbed;

        ///// <summary>
        ///// Gets or sets whether shortcut key for default action is enabled.
        ///// </summary>
        //public bool DefaultShortcutIsEnalbed
        //{
        //    get
        //    {
        //        return this.valueOfDefaultShortcutIsEnalbed;
        //    } // end get
        //    set
        //    {
        //        this.valueOfDefaultShortcutIsEnalbed = value;
        //        this.RefreshDefaultButtonMessage();
        //    } // end set
        //} // end property

        ///// <summary>
        ///// Backing field of IsDefaultButton property.
        ///// </summary>
        //private bool valueOfIsDefaultButton;

        ///// <summary>
        ///// Gets or sets whether this button is defalut or not.
        ///// </summary>
        //public bool IsDefaultButton
        //{
        //    get
        //    {
        //        return this.valueOfIsDefaultButton;
        //    } // end get
        //    set
        //    {
        //        this.valueOfIsDefaultButton = value;
        //        this.RefreshDefaultButtonMessage();
        //    } // end set
        //} // end property

        //private void RefreshDefaultButtonMessage()
        //{
        //    var thisDefaultButtonMessage = this.Template.FindName("DefaultButtonMessage", this) as TextBlock;
        //    if(thisDefaultButtonMessage!=null) thisDefaultButtonMessage.Visibility = this.valueOfIsDefaultButton.ToVisibleOrCollapsed();

        //    var shortcutZ = this.Template.FindName("shortcutZ", this) as TextBlock;
        //    if(shortcutZ!=null) shortcutZ.Visibility = this.valueOfDefaultShortcutIsEnalbed.ToVisibleOrCollapsed();

        //    var shortcutZError = this.Template.FindName("shortcutZError", this) as TextBlock;
        //    if(shortcutZError!=null) shortcutZError.Visibility = (! this.valueOfDefaultShortcutIsEnalbed ).ToVisibleOrCollapsed();
        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.RefreshDefaultButtonMessage();

        } // end sub

    } // end class
} // end namespace

