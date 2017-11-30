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

            RenderOptions.SetEdgeMode(this.frozenImage, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.frozenImage, BitmapScalingMode.NearestNeighbor);

            RenderOptions.SetEdgeMode(this.magnifyingImage, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.magnifyingImage, BitmapScalingMode.NearestNeighbor);

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

                this.magnifyingImage.Source = this.frozenImage.Source;
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

            this.frozenImage.Width = this.Width ;
            this.frozenImage.Height = this.Height ;

            this.magnifyingImage.Width = this.Width ;
            this.magnifyingImage.Height = this.Height ;

        } // end sub


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (this.isCropping)
                {
                    this.isCropping = false;
                    this.horizontalLine.Visibility = Visibility.Visible;
                    this.verticalLine.Visibility = Visibility.Visible;
                    this.cropGuidelines.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.Close();
                }
            }

        } // end sub

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                this.Close();
            }
        }

        private void FrozenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.isCropping &&
                e.RightButton == MouseButtonState.Pressed ){

                this.isCropping = false;
                this.horizontalLine.Visibility = Visibility.Visible;
                this.verticalLine.Visibility = Visibility.Visible;
                this.cropGuidelines.Visibility = Visibility.Collapsed;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.isCropping = true;
                var point = e.GetPosition(this);
                this.startPoint = new Point(point.X, point.Y);
                this.horizontalLine.Visibility = Visibility.Collapsed;
                this.verticalLine.Visibility = Visibility.Collapsed;
                this.cropGuidelines.Visibility = Visibility.Visible;
            }
            else
            {
                this.Close();
            }

        }


        private bool beLeft = false;
        private bool beTop = false;

        private void FrozenImage_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            this.verticalLine.X1 = point.X;
            this.verticalLine.X2 = point.X;

            this.horizontalLine.Y1 = point.Y;
            this.horizontalLine.Y2 = point.Y;

            if( this.beLeft )
            {
                this.beLeft =  this.guide.ActualWidth + 10 < point.X;
            }
            else
            {
                this.beLeft = this.Width < this.guide.ActualWidth + point.X + 10;
            }

            if( this.beLeft )
            {
                Canvas.SetLeft(this.guide, point.X - 10 - this.guide.ActualWidth);
                this.guideMessageRight.Visibility = Visibility.Collapsed;
                this.guideMessageLeft.Visibility = Visibility.Visible;
            }
            else
            {
                Canvas.SetLeft(this.guide, point.X + 10 );
                this.guideMessageRight.Visibility = Visibility.Visible;
                this.guideMessageLeft.Visibility = Visibility.Collapsed;
            }

            if (this.beTop)
            {
                beTop = this.guide.ActualHeight + 10 < point.Y;
            }
            else
            {
                beTop = this.Height < this.guide.ActualHeight + point.Y + 10;
            }

            if (beTop)
            {
                Canvas.SetTop(this.guide, point.Y - 10 - this.guide.ActualHeight);
            }
            else
            {
                Canvas.SetTop(this.guide, point.Y + 10 );
            }


            this.cropBounds.X = Math.Min(point.X, this.startPoint.X);
            this.cropBounds.Y = Math.Min(point.Y, this.startPoint.Y);
            this.cropBounds.Width =  Math.Abs(point.X - startPoint.X) + 1;
            this.cropBounds.Height = Math.Abs(point.Y - startPoint.Y) + 1;

            Canvas.SetLeft(this.cropGuidelines, this.cropBounds.X);
            Canvas.SetTop(this.cropGuidelines, this.cropBounds.Y);

            this.cropGuidelines.Width = this.cropBounds.Width;
            this.cropGuidelines.Height = this.cropBounds.Height;

            if (this.isCropping)
            {
                this.positionLabelLeft.Content = "(" + this.cropBounds.Left + "," + this.cropBounds.Top + ") - (" + Math.Max(point.X, this.startPoint.X) + "," + Math.Max(point.Y, this.startPoint.Y) + ") : " + this.cropBounds.Width + " x " + this.cropBounds.Height + " Pixels";
                this.positionLabelRight.Content = this.positionLabelLeft.Content;
            }
            else
            {
                this.positionLabelLeft.Content = "(" + point.X + "," + point.Y + ")";
                this.positionLabelRight.Content = this.positionLabelLeft.Content;
            }
            var magnifyingCenter = new Point(point.X - 10, point.Y - 10);

            this.magnifyingTransform.X = -10 * magnifyingCenter.X;
            this.magnifyingTransform.Y = -10 * magnifyingCenter.Y;
            this.magnifyingClip.Rect = new Rect(magnifyingCenter.X , magnifyingCenter.Y, 21, 21);


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
