using Net.Surviveplus.LightCutter;
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

namespace InsiderCutterPrototype.Desktop
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void cutButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                button.IsEnabled = false;
                await LightCutter.CutRemoteDesktopInsiderAsync();
            }
            finally
            {
                button.IsEnabled = true;
            } // end try

        } // end sub

        private async void resizeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                button.IsEnabled = false;
                await Resizer.ResizeAsync();
            }
            finally
            {
                button.IsEnabled = true;
            } // end try

        }

        private async void cutPrimaryMonitorButton_Click(object sender, RoutedEventArgs e)
        {
            var window = this;
            try
            {
                window.Visibility = Visibility.Hidden;

                await Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                });

                await LightCutter.CutPrimaryDisplay();
            }
            finally
            {
                window.Visibility = Visibility.Visible;
            } // end try			

        }

        private async void cutVmconnectButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                button.IsEnabled = false;
                await LightCutter.CutVMConnectInsiderAsync();
            }
            finally
            {
                button.IsEnabled = true;
            } // end try
        }

        private async void cutIeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                button.IsEnabled = false;
                await LightCutter.CutIeInsiderAsync();
            }
            finally
            {
                button.IsEnabled = true;
            } // end try			
        } // end sub

    } // end class
} // end namespace
