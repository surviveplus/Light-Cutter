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

namespace Net.Surviveplus.LightCutter.Desktop
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Action Panel - Light Cutter ver." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " (Preview)";
            
            if( (App.Current.MainWindow as BackgroundWindow).ShortcutA)
            {
                this.shortcutA.Visibility = Visibility.Visible;
                this.shortcutAError.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.shortcutA.Visibility = Visibility.Collapsed;
                this.shortcutAError.Visibility = Visibility.Visible;
            }
        }
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var enabled = LightCutter.LastRange.HasValue;
            this.CutSameAreaAndSaveButton.IsEnabled = enabled;
            this.CountdownCutSaveAreaAndSaveButton.IsEnabled = enabled;
        }


        private void CutAndCopyButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndCopy(this);
        }

        private void CutAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this);
        }
        private void CutSameAreaAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this);
        }

        private void CountdownCutAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this, DateTime.Now + TimeSpan.FromSeconds(3));
        }

        private void CountdownCutSaveAreaAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this, DateTime.Now + TimeSpan.FromSeconds(3));
        }
    }
}
