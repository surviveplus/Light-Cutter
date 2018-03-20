using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.Desktop
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.MainWindow = new BackgroundWindow();
            this.MainWindow.Show();
        }

        public void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs e)
        {
            this.MainWindow.WindowState = WindowState.Normal;
            this.MainWindow.Activate();

            (this.MainWindow as BackgroundWindow).ShowActionPannel();
        }
    }
}
