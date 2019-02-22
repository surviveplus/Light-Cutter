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
    /// VersionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionPage : Page
    {
        public VersionPage(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }
        private MainWindow parentWindow;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.ShowAction();
        }

        private void PrivacyStatementLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://help.surviveplus.net/archives/1239");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.versionLabel.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
