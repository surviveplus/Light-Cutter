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
        #region constructor

        /// <summary>
        /// Initialize instance
        /// </summary>
        public FullScreenWindow()
        {
            InitializeComponent();
        } // end constructor

        #endregion

        #region methods

        /// <summary>
        /// Show dialog window to display Frozen Screen image and to let the user specify a range. 
        /// </summary>
        /// <param name="frozen">Set Frozen Screen  image object.</param>
        /// <returns>Return true if the user specified the range, otherwise return false.</returns>
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
        } // end function 

        #endregion

        #region Window Events 

       /// <summary>
        /// Rate to transform to device. 
        /// </summary>
        private Point toDevice;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var source = PresentationSource.FromVisual(this.frozenImage);
            this.toDevice = new Point(
                source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22);
        } // end sub
        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        } // end sub

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Close();
        } // end sub

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        } // end sub

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.verticalLine.Y1 = 0;
            this.verticalLine.Y2 = this.Height;
            this.horizontalLine.X1 = 0;
            this.horizontalLine.X2 = this.Width;

        } // end sub

        #endregion

        #region FrozenImage Events

        private void FrozenImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isCropping )
            {
                this.isCropping = false;
                this.DialogResult = true;
            }
            this.Close();

        }

        private void FrozenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.isCropping = true;
                var point = e.GetPosition(this);
                this.startPoint = new Point( point.X, point.Y);

                this.horizontalLine.Visibility = Visibility.Collapsed;
                this.verticalLine.Visibility = Visibility.Collapsed;
                this.cropGuidelines.Visibility = Visibility.Visible;

                
            }
            else
            {
                this.Close();
            }

        }

        private void FrozenImage_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();

        }

        private void FrozenImage_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            this.verticalLine.X1 = point.X;
            this.verticalLine.X2 = point.X;

            this.horizontalLine.Y1 = point.Y;
            this.horizontalLine.Y2 = point.Y;

            this.cropBounds.X = Math.Min(point.X, this.startPoint.X);
            this.cropBounds.Y = Math.Min(point.Y, this.startPoint.Y);
            this.cropBounds.Width =  Math.Abs(point.X - startPoint.X);
            this.cropBounds.Height = Math.Abs(point.Y - startPoint.Y);

            Canvas.SetLeft(this.cropGuidelines, this.cropBounds.X);
            Canvas.SetTop(this.cropGuidelines, this.cropBounds.Y);

            this.cropGuidelines.Width = this.cropBounds.Width;
            this.cropGuidelines.Height = this.cropBounds.Height;
        }

        private Point startPoint = new Point();
        private Rect cropBounds = new Rect();
        private bool isCropping;

        #endregion

        #region Properties

        /// <summary>
        /// Get Cropped bounds as System.Drawing.Rectangle
        /// </summary>
        public System.Drawing.Rectangle CroppedBounds {
            get {
                return new System.Drawing.Rectangle(
                    (int)Math.Round(this.cropBounds.X * toDevice.X),
                    (int)Math.Round(this.cropBounds.Y * toDevice.Y),
                    (int)Math.Round(this.cropBounds.Width * toDevice.X), 
                    (int)Math.Round(this.cropBounds.Height * toDevice.Y));
            } // end get
        } // end property

        #endregion

    } // end class
} // end namespace
