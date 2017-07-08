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

            using (var s = new MemoryStream())
            {
                frozen.FrozenImage.Save(s, ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                this.frozenImage.Source = BitmapFrame.Create(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return this.ShowDialog();

        } // end sub

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            this.Close();

        }

        private void frozenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();

        }

        private void frozenImage_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
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
        }

    } // end class
} // end namespace
