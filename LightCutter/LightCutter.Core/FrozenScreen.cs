using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Core
{
    public class FrozenScreen : IDisposable
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
        ~FrozenScreen()
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
                    this.frozenImage?.Dispose();
                } // end if

                // TODO: Dispose unmanaged resources.
            } // end if

            this.disposedValue = true;
        } // end sub

        #endregion


        private Rectangle bounds ;

        private Bitmap frozenImage;

        public FrozenScreen(Rectangle bounds)
        {
            this.bounds = bounds;

            this.frozenImage = new Bitmap(this.bounds.Width, this.bounds.Height);
            using (var g = Graphics.FromImage(this.frozenImage))
            {
                g.CopyFromScreen(this.bounds.Left, this.bounds.Top, 0, 0, this.bounds.Size);
            } // end using g

        } // end constructor


        public CroppedImage Crop()
        {
            var cropped = new Bitmap(this.frozenImage);
            return new CroppedImage(cropped);
        } // end function

        public CroppedImage Crop(Rectangle part )
        {
            var cropped = new Bitmap(part.Width,part.Height);
            using (var g = Graphics.FromImage(cropped)){
                g.DrawImage(this.frozenImage, 0, 0, part, GraphicsUnit.Pixel);
            }
            return new CroppedImage(cropped);
        } // end function

    } // end class
} // end namespace
