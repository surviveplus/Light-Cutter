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
using System.Windows.Shapes;

namespace Net.Surviveplus.LightCutter.Desktop
{
    /// <summary>
    /// BackgroundWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BackgroundWindow : Window
    {
        #region constructors

        public BackgroundWindow()
        {
            InitializeComponent();
            this.ShowActionPannel();
        }

        #endregion

        #region methods

        private MainWindow main = null;
        
        public void ShowActionPannel()
        {
            if (this.main == null)
            {
                this.main = new MainWindow();
                this.main.Closed += (sender, e) => { this.main = null; };
            }
            this.main?.Show();
            if(this.main?.WindowState == WindowState.Minimized)
            {
                this.main.WindowState = WindowState.Normal;
            }
            this.main?.Activate();
        }

        #endregion

        #region Window events

        private void Window_Closed(object sender, EventArgs e)
        {
            this.main?.Close();
        }

        #endregion

        #region Notify menu events
        private void CutAndCopyAction_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndCopy(this.main);
        }

        private void CutAndSaveAction_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.main);
        }


        private void OpenMainAction_Click(object sender, RoutedEventArgs e)
        {
            this.ShowActionPannel();
        }

        private void CloseAction_Click(object sender, RoutedEventArgs e)
        {
            this.Notify.Visibility = Visibility.Hidden;
            this.Close();
        }

        #endregion

        #region Notify Icon events

        private void Notify_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            LightCutter.CutAndSave(this.main);
        }

        #endregion

    } // end class
} // end namespace
