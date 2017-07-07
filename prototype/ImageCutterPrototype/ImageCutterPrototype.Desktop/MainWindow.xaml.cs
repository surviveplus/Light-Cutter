using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace ImageCutterPrototype.Desktop
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.model;
        }
        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop,true))
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }

        private CutSizeModel model = new CutSizeModel();

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as IEnumerable<string>;
            if(files != null)
            {
                foreach (var file in files)
                {
                    var newFileName = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(file),
                        System.IO.Path.GetFileNameWithoutExtension(file) + "-" + this.model.Size.Width + "x" + this.model.Size.Height + System.IO.Path.GetExtension(file));
                    try
                    {
                        using (var originalImage = Bitmap.FromFile(file))
                        using (var newImage = LightCutter.CutImage(originalImage, model.Size, model.Scale))
                        {
                            newImage.Save(newFileName, originalImage.RawFormat);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ERROR: " + file );
                        Debug.WriteLine(ex.ToString());
                    }
                }

            } // end if

        }

    }
}
