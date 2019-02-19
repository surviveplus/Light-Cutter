using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.Parts
{
    /// <summary>
    /// Static class which is defined extension methods.
    /// </summary>
    public static class BooleanExtensions
    {

        /// <summary>
        /// Get Visibility.Visible or Collapsed value.
        /// </summary>
        /// <param name="me">The instance of the type which is added this extension method.</param>
        /// <returns>
        /// Returns Visibility.Visible if true; otherwise returns Visibility.Collapsed.
        /// </returns>
        public static Visibility ToVisibleOrCollapsed(this Boolean me)
        {
            if (me)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }

        } // end function
    } // end class
} // end namespace
