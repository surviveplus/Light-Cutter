using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands
{
    public class ActionCommandDisplay
    {
        public static IEnumerable<object> Create(params object[] items)
        {
            return items;
        }
    }
}
