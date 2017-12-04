using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI
{

    /// <summary>
    /// Static class which is defined extension methods.
    /// </summary>
    public static class RectExtensions
    {

        /// <summary>
        /// Get System.Drawing.Rectangle from System.Windows.Rect
        /// </summary>
        /// <param name="me">The instance of the type which is added this extension method.</param>
        /// <returns>
        /// Returns System.Drawing.Rectangle .
        /// </returns>
        public static System.Drawing.Rectangle ToRectangle(this Rect me)
        {
            if (me == null) throw new ArgumentNullException("me");

            return new System.Drawing.Rectangle(
                (int)Math.Round(me.X), (int)Math.Round(me.Y),
                (int)Math.Round(me.Width), (int)Math.Round(me.Height));

        } // end function
    } // end class
} // end namespace
