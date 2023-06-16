using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Core
{
    public class Screen
    {
        public static FrozenScreen Freeze( DateTime? time = null) {
            if( time.HasValue == false ) { time = DateTime.Now; }

            var bounds = new Rectangle();
            foreach (var b in from s in System.Windows.Forms.Screen.AllScreens select s.Bounds)
            {
                Debug.WriteLine($"({b.Left},{b.Top})-({b.Right},{b.Bottom}) : {b.Width}x{b.Height}");
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var timeSpan = time.Value - DateTime.Now;
            if (timeSpan < TimeSpan.FromSeconds(0)) { timeSpan = TimeSpan.FromSeconds(0); }

            System.Threading.Thread.Sleep(timeSpan);
            Debug.WriteLine($"FrozenScreen : ({bounds.Left},{bounds.Top})-({bounds.Right},{bounds.Bottom}) : {bounds.Width}x{bounds.Height}");
            return new FrozenScreen(bounds);

        } // end function

    } // end class
} // end namespace
