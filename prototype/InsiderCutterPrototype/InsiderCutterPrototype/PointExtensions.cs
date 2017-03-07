using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter
{
    /// <summary>
    /// Static class which is defined extension methods.
    /// </summary>
    public static class PointExtensions
    {

        /// <summary>
        /// Get a System.Drawing.Point value from a System.Windows.Point value.
        /// </summary>
        /// <param name="me">The instance of the type which is added this extension method.</param>
        /// <returns>
        /// Returns a System.Drawing.Point value.
        /// </returns>
        public static Point ToPoint(this System.Windows.Point me)
        {
            if (me == null) throw new ArgumentNullException("me");
            return new Point((int)me.X, (int)me.Y);
        } // end function
    } // end class
} // end namespace
