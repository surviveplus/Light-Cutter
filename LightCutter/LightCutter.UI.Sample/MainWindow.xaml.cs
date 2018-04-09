using Net.Surviveplus.LightCutter.UI.ViewModels;
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

            this.shortcutBox.ItemsSource = this.hotkeys;
            //this.shortcutBox.SelectedIndex = 0;
            this.shortcutBox.Text = "Win + Shift + A";

            this.shortcutBox2.ItemsSource = this.hotkeys;
            this.shortcutBox2.Text = "Win + Shift + Z";
        }

        private List<HotkeyViewModel> hotkeys = HotkeyViewItems.CreateAll();

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if( this.Width < 400)
            {
                VisualStateManager.GoToElementState(this.mainGrid, "Small", true);
                //this.wait.Visibility = Visibility.Collapsed;
            }
            else
            {
                VisualStateManager.GoToElementState(this.mainGrid, "Full", true);
                //this.wait.Visibility = Visibility.Visible;
            }
        }
    }


}
