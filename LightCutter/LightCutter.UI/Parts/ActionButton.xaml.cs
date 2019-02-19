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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        } // end sub

        public event EventHandler<EventArgs> Click;


        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.Click?.Invoke(this, EventArgs.Empty);
        } // end sub

    } // end class
} // end namespace

