﻿using System;
using System.Collections.Generic;
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
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var timeSpan = time.Value - DateTime.Now;
            if (timeSpan < TimeSpan.FromSeconds(0)) { timeSpan = TimeSpan.FromSeconds(0); }

            System.Threading.Thread.Sleep(timeSpan);
            return new FrozenScreen(bounds);

        } // end function

    } // end class
} // end namespace
