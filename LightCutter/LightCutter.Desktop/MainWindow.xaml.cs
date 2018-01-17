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
            this.ShowAction();
            
        }

        public void GoBack()
        {
            this.mainFrame.GoBack();
        }

        public void ShowAction() {
            this.mainFrame.Content = new Pages.ActionPage(this);
        }

        public void ShowSetting()
        {
            this.mainFrame.Content = new Pages.SettingsPage(this);
        }

    }
}
