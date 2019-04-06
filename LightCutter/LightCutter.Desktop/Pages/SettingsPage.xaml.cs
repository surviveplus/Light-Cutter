using Net.Surviveplus.LightCutter.Commands.Sharing;
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
using static System.Environment;

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
            this.shortcutOpenActionPanelBox.Text = Settings.Default.ShortcutOpenActionPanel;

            this.shortcutStartDefaultActionBox.ItemsSource = this.hotkeys;
            this.shortcutStartDefaultActionBox.Text = Settings.Default.ShortcutStartDefaultAction;
        }
        private MainWindow parentWindow;
        private List<HotkeyViewModel> hotkeys = HotkeyViewItems.CreateAll();

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.ShortcutOpenActionPanel != this.shortcutOpenActionPanelBox.Text ||
                Settings.Default.ShortcutStartDefaultAction != this.shortcutStartDefaultActionBox.Text) {

                Settings.Default.ShortcutOpenActionPanel = this.shortcutOpenActionPanelBox.Text;
                Settings.Default.ShortcutStartDefaultAction = this.shortcutStartDefaultActionBox.Text;

                var b = App.Current.MainWindow as BackgroundWindow;
                b.SetHotkey();
            }

            this.parentWindow.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.SaveAndUpdateCommandsSettings();
        }

        private void VersionInformationLink_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.ShowVersionInformation();
        }

        private void DefaultFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var d = new System.Windows.Forms.FolderBrowserDialog())
            {
                var folder = this.defaultFolderBox.Text;

                var specialFolder = SaveFileCommand.GetSupportedSpecialFolder(folder);
                if (specialFolder != null)
                {
                    folder = specialFolder.FullName;
                }

                if (System.IO.Directory.Exists(folder))
                {
                    d.SelectedPath = folder;
                } // end if

                if( d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.defaultFolderBox.Text = d.SelectedPath;
                } // end if
            } // end using (d)
        }
    }
}
