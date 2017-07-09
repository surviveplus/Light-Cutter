using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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

namespace Net.Surviveplus.LightCutter.UI
{
    /// <summary>
    /// FullScreenWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FullScreenWindow : Window
    {
        public FullScreenWindow()
        {
            InitializeComponent();
        }
        
        public bool? ShowFrozenScreen( Core.FrozenScreen frozen)
        {
            this.Left = frozen.Bounds.Left;
            this.Top = frozen.Bounds.Top;
            this.Width = frozen.Bounds.Width;
            this.Height = frozen.Bounds.Height;

            this.cropGuidelines.Visibility = Visibility.Collapsed;

            using (var s = new MemoryStream())
            {
                frozen.FrozenImage.Save(s, ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                this.frozenImage.Source = BitmapFrame.Create(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return this.ShowDialog();

        } // end sub

        private Point p;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var source = PresentationSource.FromVisual(this.frozenImage);
            this.p = new Point(
            source.CompositionTarget.TransformToDevice.M11,
            source.CompositionTarget.TransformToDevice.M22);

        } // end sub


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            this.Close();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void frozenImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isCropping )
            {
                this.isCropping = false;
                this.DialogResult = true;
            }
            this.Close();

        }

        private void frozenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.isCropping = true;
                var point = e.GetPosition(this);
                this.startPoing = new System.Drawing.PointF( (float)point.X, (float)point.Y);

                this.horizontalLine.Visibility = Visibility.Collapsed;
                this.verticalLine.Visibility = Visibility.Collapsed;
                this.cropGuidelines.Visibility = Visibility.Visible;

                
            }
            else
            {
                this.Close();
            }

        }

        private void frozenImage_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();

        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.verticalLine.Y1 = 0;
            this.verticalLine.Y2 = this.Height;
            this.horizontalLine.X1 = 0;
            this.horizontalLine.X2 = this.Width;

        }

        private void frozenImage_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            this.verticalLine.X1 = point.X;
            this.verticalLine.X2 = point.X;

            this.horizontalLine.Y1 = point.Y;
            this.horizontalLine.Y2 = point.Y;

            this.cropBounds.X = (float)Math.Min(point.X, this.startPoing.X);
            this.cropBounds.Y = (float)Math.Min(point.Y, this.startPoing.Y);
            this.cropBounds.Width =  (float)Math.Abs(point.X - (double) startPoing.X);
            this.cropBounds.Height = (float)Math.Abs(point.Y - (double)startPoing.Y);

            Canvas.SetLeft(this.cropGuidelines, this.cropBounds.X);
            Canvas.SetTop(this.cropGuidelines, this.cropBounds.Y);

            this.cropGuidelines.Width = this.cropBounds.Width;
            this.cropGuidelines.Height = this.cropBounds.Height;
        }

        private System.Drawing.PointF startPoing = new System.Drawing.PointF();
        private System.Drawing.RectangleF cropBounds = new System.Drawing.RectangleF();
        private bool isCropping;

        public System.Drawing.Rectangle CroppedBounds {
            get {
                return new System.Drawing.Rectangle(
                    (int)(this.cropBounds.X * p.X),
                    (int)(this.cropBounds.Y * p.Y),
                    (int)(this.cropBounds.Width * p.X), 
                    (int)(this.cropBounds.Height * p.Y));
            }
        }

    } // end class
} // end namespace
