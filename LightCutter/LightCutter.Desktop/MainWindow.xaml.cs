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

            // TODO: 
            //using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze())
            //using (var cropped = frozen?.Crop())
            //using (var bitmap = cropped?.GetBitmap())
            //{

            //    var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
            //    bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
            //}
             this.Close();
        }
    }
}
