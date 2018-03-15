using Net.Surviveplus.LightCutter.Desktop.Properties;
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

namespace Net.Surviveplus.LightCutter.Desktop.Pages
{
    /// <summary>
    /// SettingsPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;


            this.shortcutOpenActionPanelBox.ItemsSource = this.hotkeys;
            this.shortcutOpenActionPanelBox.Text = "Win + Shift + A";

            this.shortcutStartDefaultActionBox.ItemsSource = this.hotkeys;
            this.shortcutStartDefaultActionBox.Text = "Win + Shift + Z";
        }
        private MainWindow parentWindow;
        private List<HotkeyViewModel> hotkeys = HotkeyViewItems.CreateAll();

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
