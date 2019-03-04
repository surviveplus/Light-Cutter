using Net.Surviveplus.LightCutter.UI.Parts;
using Net.Surviveplus.LightCutter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


            var a = new DefaultShortcutViewModel {
                Hotkey = new HotkeyViewModel { Win = Visibility.Visible, Shift = Visibility.Visible, Key = HotKeys.A },
                HotkeyVisibility = Visibility.Visible,
                ErrorVisibility = Visibility.Collapsed };

            var b = new DefaultShortcutViewModel
            {
                Hotkey = a.Hotkey,
                HotkeyVisibility = Visibility.Collapsed,
                ErrorVisibility = Visibility.Visible
            };

            this.action1.DataContext = new ActionViewModel
            {
                AccessText = "_1.",
                DefaultShortcut = a,
                DefaultShortcutVisibility = Visibility.Visible,
            };
            this.action1.MustUac = true;
            this.action2.DataContext = new ActionViewModel
            {
                AccessText = "_2.",
                DefaultShortcut = b,
                DefaultShortcutVisibility = Visibility.Visible,
            };

            this.action3.DataContext = new ActionViewModel
            {
                AccessText = "_3.",
                DefaultShortcut = a,
                DefaultShortcutVisibility = Visibility.Collapsed,
            };


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

        private void Action3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine((sender as ActionButton).Content);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var edit = (sender as CheckBox).IsChecked.Value;


            this.action1.ShowDefaultActionSelection = edit;
            this.action2.ShowDefaultActionSelection = edit;
            this.action3.ShowDefaultActionSelection = edit;

        }

        private void Action1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Action1_Click(object sender, EventArgs e)
        {
            var w = new EditActionWindow();
            w.ShowDialog();
        }

        private void Action1_EditButtonClick(object sender, EventArgs e)
        {
            var w = new EditActionWindow();
            w.ShowDialog();

        }
    }


}
