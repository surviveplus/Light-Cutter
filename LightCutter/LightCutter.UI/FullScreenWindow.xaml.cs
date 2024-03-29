﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
            public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

            #endregion

        } // end class

        #region constructor

        /// <summary>
        /// Initialize instance
        /// </summary>
        public FullScreenWindow(bool guideBackgroundTransparent, int gridPixel)
        {
            this.GridPixel = gridPixel;
            this.GuideBackgroundTransparent = guideBackgroundTransparent;

            InitializeComponent();

            RenderOptions.SetEdgeMode(this.frozenImage, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.frozenImage, BitmapScalingMode.NearestNeighbor);

            RenderOptions.SetEdgeMode(this.magnifyingImage, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.magnifyingImage, BitmapScalingMode.NearestNeighbor);

        } // end constructor

        #endregion

        #region methods

        private System.Drawing.Rectangle originalBounds;

        /// <summary>
        /// Show dialog window to display Frozen Screen image and to let the user specify a range. 
        /// </summary>
        /// <param name="frozen">Set Frozen Screen  image object.</param>
        /// <returns>Return true if the user specified the range, otherwise return false.</returns>
        public bool? ShowFrozenScreen( Core.FrozenScreen frozen)
        {
            
            this.originalBounds = frozen.Bounds;
            this.Width = 0;
            this.Height = 0;
            // Resize and Move on Window_Loaded

            using (var s = new MemoryStream())
            {
                frozen.FrozenImage.Save(s, ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                this.frozenImage.Source = BitmapFrame.Create(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                this.magnifyingImage.Source = this.frozenImage.Source;
            }
            return this.ShowDialog();
        } // end function 

        public System.Drawing.Point CroppedOffest { get; private set; } = new System.Drawing.Point();

        public bool? ShowCroppedImage(Core.CroppedImage cropped)
        {
            // TODO: move to Screen class
            var bounds = new System.Drawing.Rectangle();
            foreach (var b in from s in System.Windows.Forms.Screen.AllScreens select s.Bounds)
            {
                bounds = System.Drawing.Rectangle.Union(bounds, b);
            } // next b

            this.originalBounds = bounds;
            this.Width = 0;
            this.Height = 0;
            // Resize and Move on Window_Loaded

            using (var s = new MemoryStream())
            using(var bitmap = new System.Drawing.Bitmap(bounds.Width,bounds.Height))
            {
                using (var g = System.Drawing.Graphics.FromImage(bitmap))
                using (var b = cropped.GetBitmap())
                {
                    // primariy monitor or all screen.
                    this.CroppedOffest = new System.Drawing.Point();
                    var primary = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    if(b.Width < primary.Width && b.Height < primary.Height)
                    {
                        this.CroppedOffest = new System.Drawing.Point( primary.Location.X - this.originalBounds.Location.X , primary.Location.Y - this.originalBounds.Location.Y);
                    } // end if

                    g.DrawImage(b, this.CroppedOffest);
                } // end using (g, b)

                bitmap.Save(s, ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                this.frozenImage.Source = BitmapFrame.Create(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                this.magnifyingImage.Source = this.frozenImage.Source;
            } // end using (s,bitmap)
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

            var size = new Size(this.originalBounds.Width / this.cropping.toDevice.X, this.originalBounds.Height / this.cropping.toDevice.Y);

            this.frozenImage.Width = size.Width;
            this.frozenImage.Height = size.Height;

            this.magnifyingImage.Width = size.Width;
            this.magnifyingImage.Height = size.Height;

            this.magnifyingScale.ScaleX = 10 * this.cropping.toDevice.X;
            this.magnifyingScale.ScaleY = 10 * this.cropping.toDevice.Y;

            this.DataContext = this.cropping;
            this.guide.Visibility = Visibility.Hidden;

            if (! this.GuideBackgroundTransparent)
            {
                this.guide.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);

                var black = new SolidColorBrush(Colors.Black);
                foreach (var item in
                    guideMessageRight.Children.ToEnumerable<UIElement>().Union(
                        guideMessageLeft.Children.ToEnumerable<UIElement>()).Union( 
                        new UIElement[] { this.titleLabelLeft, this.titleLabelRight}) )
                {
                    var label = item as Label;
                    if(label != null) { label.Foreground = black; }

                    var textBlock = item as TextBlock;
                    if (textBlock != null) { textBlock.Foreground = black; }
                }
            } // end if

            this.gridPixelLeft.Text = this.GridPixel.ToString();
            this.gridPixelRight.Text = this.gridPixelLeft.Text;

            NativeMethods.SetWindowPos(new WindowInteropHelper(this).Handle, IntPtr.Zero, this.originalBounds.Left, this.originalBounds.Top, this.originalBounds.Width, this.originalBounds.Height, 0);
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

        public int GridPixel {
            get => this.cropping.GridPixel;
            set => this.cropping.GridPixel = value;
        } 

        public bool GuideBackgroundTransparent { get; set; } = true;


        /// <summary>
        /// Get Cropped bounds as System.Drawing.Rectangle
        /// </summary>
        public System.Drawing.Rectangle CroppedBounds {
            get {
                var r = this.cropping.BoundsToDevice.ToRectangle();
                r.Offset(new System.Drawing.Point ( -1 * this.CroppedOffest.X, -1 * this.CroppedOffest.Y));
                return r;
            } // end get
        } // end property

        #endregion

    } // end class

    /// <summary>
    /// Static class which is defined extension methods for UIElementCollection.
    /// </summary>
    public static partial class UIElementCollectionExtensions
    {

        /// <summary>
        /// Return the IEnumerable&lt;T&gt; for a classic collection that do not implement IEnumerable&lt;T&gt; but it is possible to be set on foreach.
        /// </summary>
        /// <typeparam name="T">The type of this elements.</typeparam>
        /// <param name="me">The instance of the type which is added this extension method. Set a null reference (Nothing in Visual Basic), to return empty IEnumerable&lt;T&gt;.</param>
        /// <returns>
        /// Return the IEnumerable&lt;T&gt;.
        /// </returns>
        public static IEnumerable<T> ToEnumerable<T>(this UIElementCollection me)
        {
            if (me != null)
            {
                dynamic list = me;

                foreach (T item in list)
                {
                    yield return item;
                } // next item
            } // end if

        } // end function
    } // end class

} // end namespace
