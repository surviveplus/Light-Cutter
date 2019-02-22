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

            this.shortcutA.Visibility = b.ShortcutOpenActionPanel.Enabled.ToVisibleOrCollapsed();
            this.shortcutA.DataContext = b.ShortcutOpenActionPanelViewModel;
            this.shortcutAError.Visibility =(! b.ShortcutOpenActionPanel.Enabled ).ToVisibleOrCollapsed();

            var defaultShortcut = new DefaultShortcutViewModel
            {
                HotkeyVisibility = b.ShortcutStartDefaultAction.Enabled.ToVisibleOrCollapsed(),
                Hotkey = b.ShortcutStartDefaultActionViewModel,
                ErrorVisibility = (!b.ShortcutStartDefaultAction.Enabled).ToVisibleOrCollapsed()
            };

            this.CutAndCopyButton.DataContext = new ActionViewModel { AccessText = "_1.", Name = "CutAndCopy", DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == "CutAndCopy" ? Visibility.Visible : Visibility.Collapsed };
            this.CutAndSaveButton.DataContext = new ActionViewModel { AccessText = "_2.", Name = "CutAndSave", DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == "CutAndSave" ? Visibility.Visible : Visibility.Collapsed };
            this.CutSameAreaAndSaveButton.DataContext = new ActionViewModel { AccessText = "_3.", Name = "CutSameAreaAndSave", DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == "CutSameAreaAndSave" ? Visibility.Visible : Visibility.Collapsed };
            this.CountdownCutAndSaveButton.DataContext = new ActionViewModel { AccessText = "_4.", Name = "CountdownCutAndSave", DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == "CountdownCutAndSave" ? Visibility.Visible : Visibility.Collapsed };
            this.CountdownCutSaveAreaAndSaveButton.DataContext = new ActionViewModel { AccessText = "_5.", Name = "CountdownCutSaveAreaAndSave", DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == "CountdownCutSaveAreaAndSave" ? Visibility.Visible : Visibility.Collapsed };
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var enabled = LightCutter.LastRange.HasValue;
            this.CutSameAreaAndSaveButton.ButtonIsEnabled = enabled;
            this.CountdownCutSaveAreaAndSaveButton.ButtonIsEnabled = enabled;
        }

        private void CutAndCopyButton_Click(object sender, EventArgs e)
        {
            LightCutter.CutAndCopy(this.parentWindow);
        }

        private void CutAndSaveButton_Click(object sender, EventArgs e)
        {
            LightCutter.CutAndSave(this.parentWindow);
        }
        private void CutSameAreaAndSaveButton_Click(object sender, EventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this.parentWindow);
        }

        private void CountdownCutAndSaveButton_Click(object sender, EventArgs e)
        {
            LightCutter.CutAndSave(this.parentWindow, DateTime.Now + TimeSpan.FromSeconds(Settings.Default.DefaultWaitTimeSeconds));
        }

        private void CountdownCutSaveAreaAndSaveButton_Click(object sender, EventArgs e)
        {
            LightCutter.CutSameAreaAndSave(this.parentWindow, DateTime.Now + TimeSpan.FromSeconds(Settings.Default.DefaultWaitTimeSeconds));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.EditButton.IsChecked = false;
            this.parentWindow.ShowSetting();
        }

        private void RefreshEditMode()
        {
            var button = this.EditButton;
            var edit = button.IsChecked.Value;

            this.CutAndCopyButton.ShowDefaultActionSelection = edit;
            this.CutAndSaveButton.ShowDefaultActionSelection = edit;
            this.CutSameAreaAndSaveButton.ShowDefaultActionSelection = edit;
            this.CountdownCutAndSaveButton.ShowDefaultActionSelection = edit;
            this.CountdownCutSaveAreaAndSaveButton.ShowDefaultActionSelection = edit;
        }

        private void EditButton_Checked(object sender, RoutedEventArgs e)
        {
            this.RefreshEditMode();
        }

        private void EditButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.RefreshEditMode();
        }

        private void ActionButtons_IsDefaultChanged(object sender, EventArgs e)
        {
            var actionButton = sender as UI.Parts.ActionButton;
            var model = actionButton.DataContext as ActionViewModel;
            var name = model.Name;

            if(model.DefaultShortcutVisibility == Visibility.Visible)
            {
                Settings.Default.DefaultActionName = name;
                Settings.Default.Save();
            }

        }

        private void HelpLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/surviveplus/Light-Cutter/blob/master/HowToUse/HowToUse.md");

        }
    }
}
