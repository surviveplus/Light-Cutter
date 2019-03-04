using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.Desktop
{
    class WindowHide : IDisposable
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
        ~WindowHide()
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
                    // Dispose managed resources.
                    if(this.parent != null)
                    {
                        try
                        {
                            this.parent.Left = this.originalLocation.X;
                            this.parent.Top = this.originalLocation.Y;
                            this.parent.ShowInTaskbar = true;
                            this.parent.Opacity = 1;
                            this.parent.Show();

                        }
                        catch {}
                    }
                } // end if

                // Dispose unmanaged resources.
            } // end if

            this.disposedValue = true;
        } // end sub

        #endregion

        private Window parent;
        private Point originalLocation;

        public WindowHide(Window parent)
        {
            this.parent = parent;
            if(this.parent == null)
            {
                return;
            }

            this.parent.ShowInTaskbar = false;

            this.originalLocation = new Point(this.parent.Left, this.parent.Top);

            var bounds = new System.Drawing.Rectangle();
            foreach (var b in from s in System.Windows.Forms.Screen.AllScreens select s.Bounds)
            {
                bounds = System.Drawing.Rectangle.Union(bounds, b);
            } // next b
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(200));

            this.parent.Left = bounds.Right + 100;
            this.parent.Top = bounds.Bottom + 100;
            this.parent.Hide();

        } // end constructor


    }
}
