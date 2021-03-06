﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Core
{
    public class CroppedImage : IDisposable
    {
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        } // end sub

        /// <summary>
        /// Destruct instance of the class.
        /// </summary>
        ~CroppedImage()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Backing field to track whether Dispose has been called.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose managed and unmanaged resources of this instance.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources can be disposed. If disposing equals false, only unmanaged resources can be disposed. </param>
        protected virtual void Dispose(bool disposing)
        {

            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources.
                    this.bitmap?.Dispose();
                } // end if

                // TODO: Dispose unmanaged resources.
            } // end if

            this.disposedValue = true;
        } // end sub

        #endregion

        private Bitmap bitmap;

        public CroppedImage(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public Bitmap GetBitmap()
        {
            return new Bitmap(this.bitmap);
        } // end function

        public CroppedImage Crop()
        {
            var cropped = new Bitmap(this.bitmap);
            return new CroppedImage(cropped);
        } // end function


        public CroppedImage Crop(Rectangle part)
        {
            var bounds = new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height);
            bounds.Intersect(part);

            var cropped = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(cropped))
            {
                g.DrawImage(this.bitmap, 0, 0, bounds, GraphicsUnit.Pixel);
            }
            return new CroppedImage(cropped);
        } // end function


    } // end class
} // end namespace
