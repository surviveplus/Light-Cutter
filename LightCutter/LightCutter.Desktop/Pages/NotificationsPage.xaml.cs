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
    /// NotificationsPage.xaml の相互作用ロジック
    /// </summary>
    public partial class NotificationsPage : Page
    {
        public NotificationsPage(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }
        private MainWindow parentWindow;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.GoBack();

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var removing = button.Tag as Models.NotificationModel;
            if (removing != null)
            {
                LightCutter.Notifications.Remove(removing);
            }
        }
    }
}
