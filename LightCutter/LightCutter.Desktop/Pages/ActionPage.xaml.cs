using Net.Surviveplus.LightCutter.Desktop.Properties;
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

namespace Net.Surviveplus.LightCutter.Desktop.Pages
{
    /// <summary>
    /// ActionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ActionPage : Page
    {
        public ActionPage( MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }
        private MainWindow parentWindow;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var b = App.Current.MainWindow as BackgroundWindow;

            this.shortcutA.Visibility = b.ShortcutA.ToVisibleOrCollapsed();
            this.shortcutAError.Visibility =(! b.ShortcutA ).ToVisibleOrCollapsed();

            this.shortcutZ.Visibility = b.ShortcutZ.ToVisibleOrCollapsed();
            this.shortcutZError.Visibility = (!b.ShortcutZ).ToVisibleOrCollapsed();
        } 

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var enabled = LightCutter.LastRange.HasValue;
            this.CutSameAreaAndSaveButton.IsEnabled = enabled;
            this.CountdownCutSaveAreaAndSaveButton.IsEnabled = enabled;
        }

        private void CutAndCopyButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndCopy(this.parentWindow);
        }

        private void CutAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.parentWindow);
        }
        private void CutSameAreaAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this.parentWindow);
        }

        private void CountdownCutAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.parentWindow, DateTime.Now + TimeSpan.FromSeconds(Settings.Default.DefaultWaitTimeSeconds));
        }

        private void CountdownCutSaveAreaAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this.parentWindow, DateTime.Now + TimeSpan.FromSeconds(Settings.Default.DefaultWaitTimeSeconds));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.ShowSetting();
        }
    }
}
