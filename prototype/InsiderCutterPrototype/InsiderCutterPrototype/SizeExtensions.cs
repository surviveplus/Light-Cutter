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
    public static class SizeExtensions
    {
        /// <summary>
        /// Get a System.Drawing.Size value from a System.Windows.Size value.
        /// </summary>
        /// <param name="me">The instance of the type which is added this extension method.</param>
        /// <returns>
        /// Returns a System.Drawing.Size value.
        /// </returns>
        public static Size ToSize(this System.Windows.Size me)
        {
            if (me == null) throw new ArgumentNullException("me");
            return new Size((int)me.Width, (int)me.Height);
        } // end function
    } // end class
} // end namespace
