using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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

        /// <summary>
        /// Allows managed code to call unmanaged functions with Platform Invocation Services (PInvoke).
        /// </summary>
        internal static class NativeMethods
        {
            #region Win32 API Definitions

            // TODO: Insert the code of Declare of DllImport. (see static code analysis CA1060)

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            #endregion

        } // end class

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

        private Core.FrozenScreen frozen;

        /// <summary>
        /// Show dialog window to display Frozen Screen image and to let the user specify a range. 
        /// </summary>
        /// <param name="frozen">Set Frozen Screen  image object.</param>
        /// <returns>Return true if the user specified the range, otherwise return false.</returns>
        public bool? ShowFrozenScreen( Core.FrozenScreen frozen)
        {
            this.frozen = frozen;

            this.Left = frozen.Bounds.Left;
            this.Top = frozen.Bounds.Top;
            this.Width = frozen.Bounds.Width;
            this.Height = frozen.Bounds.Height;


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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var source = PresentationSource.FromVisual(this.frozenImage);
            this.cropping.toDevice = new Point(
                source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22);

            var size = new Size(this.frozen.Bounds.Width / this.cropping.toDevice.X, this.frozen.Bounds.Height / this.cropping.toDevice.Y);

            this.frozenImage.Width = size.Width;
            this.frozenImage.Height = size.Height;

            this.magnifyingImage.Width = size.Width;
            this.magnifyingImage.Height = size.Height;

            this.magnifyingScale.ScaleX = 10 * this.cropping.toDevice.X;
            this.magnifyingScale.ScaleY = 10 * this.cropping.toDevice.Y;

            this.DataContext = this.cropping;
            this.guide.Visibility = Visibility.Hidden;

            NativeMethods.SetForegroundWindow(new WindowInteropHelper(this).Handle);
        } // end sub



        //private bool Ctrl { get; set; }

        /// <summary>
        /// Backing field of Ctrl property.
        /// </summary>
        private bool valueOfCtrl;

        /// <summary>
        /// Gets or sets Ctrl.
        /// </summary>
        public bool Ctrl
        {
            get
            {
                return this.valueOfCtrl;
            } // end get
            set
            {
                this.valueOfCtrl = value;
                this.cropping.KeepGrid = value;
            } // end set
        } // end property

        //private bool Shift { get; set; }

        /// <summary>
        /// Backing field of Shift property.
        /// </summary>
        private bool valueOfShift;

        /// <summary>
        /// Gets or sets Shift.
        /// </summary>
        public bool Shift
        {
            get
            {
                return this.valueOfShift;
            } // end get
            set
            {
                this.valueOfShift = value;
                this.cropping.KeepSqure = value;
            } // end set
        } // end property

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            this.Ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            this.Shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

            if (this.cropping.IsCropping)
            {
                this.cropping.Point = this.lastPoint;
                this.cropping.UpdateBounds();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            this.Shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

            if (e.Key == Key.Escape)
            {
                if (this.cropping.IsCropping)
                {
                    this.cropping.IsCropping = false;
                }
                else
                {
                    this.Close();
                }
            }

            if (this.cropping.IsCropping)
            {
                this.cropping.Point = this.lastPoint;
                this.cropping.UpdateBounds();
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
            if (this.cropping.IsCropping )
            {
                this.cropping.IsCropping = false;
                this.DialogResult = true;
                this.Close();
            }
            else if(this.mustClose)
            {
                this.Close();
            }
        }

        private bool mustClose = false;

        private void FrozenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.cropping.IsCropping &&
                e.RightButton == MouseButtonState.Pressed ){

                this.cropping.IsCropping = false;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.cropping.IsCropping = true;
                var point = e.GetPosition(this);
                this.cropping.StartPoint = new Point(point.X, point.Y);
                this.cropping.UpdateBounds();
            }
            else
            {
                this.mustClose = true;
            }

        }

        private Point lastPoint;

        private void FrozenImage_MouseMove(object sender, MouseEventArgs e)
        {
            this.lastPoint = e.GetPosition(this);
            this.cropping.Point = this.lastPoint;
            this.cropping.UpdateBounds();
            this.cropping.UpdateGuilde(this.Width, this.Height, this.guide.ActualWidth, this.guide.ActualHeight);
            this.guide.Visibility = Visibility.Visible;
        }

        private Cropping cropping = new Cropping();

        #endregion

        #region Properties

        /// <summary>
        /// Get Cropped bounds as System.Drawing.Rectangle
        /// </summary>
        public System.Drawing.Rectangle CroppedBounds {
            get {
                return this.cropping.BoundsToDevice.ToRectangle();
            } // end get
        } // end property

        #endregion

    } // end class
} // end namespace
