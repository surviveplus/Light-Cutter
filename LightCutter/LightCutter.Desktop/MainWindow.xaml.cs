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

            #region Change window size  ( here for WindowStartupLocation="CenterScreen" )

            Func<double, double, double, double> getValueOrDefault = (value, min, defaultValue) => {
                if (value < min)
                {
                    return defaultValue;
                }
                else
                {
                    return value;
                }
            };
            this.Width = getValueOrDefault(Settings.Default.LastMainWindowWidth, this.MinWidth, this.Width);
            this.Height = getValueOrDefault(Settings.Default.LastMainWindowHeight, this.MinHeight, this.Height);

            #endregion

        } // end constructor

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Light Cutter";
            this.ShowAction();
        }

        public void GoBack()
        {
            this.mainFrame.GoBack();
        }

        public void ShowAction() {
            this.mainFrame.Content = new Pages.ActionPage(this);
            if (this.mainFrame.CanGoBack){ this.mainFrame.RemoveBackEntry(); }
        }

        public void ShowSetting()
        {
            this.mainFrame.Content = new Pages.SettingsPage(this);
        }

        public void ShowVersionInformation()
        {
            this.mainFrame.Content = new Pages.VersionPage(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.LastMainWindowWidth = this.Width;
            Settings.Default.LastMainWindowHeight = this.Height;
            Settings.Default.SaveAndUpdateCommandsSettings();

            if (App.Current.MainWindow != null)
            {
                e.Cancel = true;
                this.Hide();
                this.ShowAction();
            }
        }
    }
}
