using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ShareImage
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
            Debug.WriteLine(DataTransferManager.IsSupported());
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += Manager_DataRequested;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested -= Manager_DataRequested;
        }
        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private async void Manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Environment.CurrentDirectory);
            StorageFile imageFile = await folder.GetFileAsync("sample.png");

            args.Request.Data.Properties.Title = "Sample Image";
            args.Request.Data.Properties.Description = "Share from prototype app for Light Cutter.";
            args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
        }


    }
}
