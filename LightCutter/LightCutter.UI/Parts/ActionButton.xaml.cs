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
            this.RefreshDefaultButtonMessage();
        }

        /// <summary>
        /// Backing field of IsDefaultButton property.
        /// </summary>
        private bool valueOfIsDefaultButton;

        /// <summary>
        /// Gets or sets whether this button is defalut or not.
        /// </summary>
        public bool IsDefaultButton
        {
            get
            {
                return this.valueOfIsDefaultButton;
            } // end get
            set
            {
                this.valueOfIsDefaultButton = value;
                this.RefreshDefaultButtonMessage();
            } // end set
        } // end property

        private void RefreshDefaultButtonMessage()
        {
            this.DefaultButtonMessage.Visibility = this.valueOfIsDefaultButton ? Visibility.Visible : Visibility.Collapsed;
        }
    } // end class
} // end namespace

