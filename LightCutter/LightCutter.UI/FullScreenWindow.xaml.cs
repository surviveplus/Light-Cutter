﻿using System;
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

        } // end sub


        private bool ctrl = false;
        private bool shift = false;

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            this.ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            this.shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            this.shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

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
        }

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
            }
            else
            {
                this.Close();
            }

        }


        private void FrozenImage_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            this.cropping.Point = e.GetPosition(this);
            this.cropping.Bounds = Cropping.GetRect(this.cropping.StartPoint, this.cropping.Point, this.cropping.toDevice, this.cropping.IsCropping, this.ctrl, this.shift);
            this.cropping.UpdateGuilde(this.Width, this.Height, this.guide.ActualWidth, this.guide.ActualHeight);
            this.cropping.UpdatePositionLabel();
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
