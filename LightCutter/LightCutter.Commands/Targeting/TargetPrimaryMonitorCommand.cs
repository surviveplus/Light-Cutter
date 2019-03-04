using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetPrimaryMonitorCommand : IActionCommand
    {
        public static TargetPrimaryMonitorCommand FromCommand(string command)
        {
            if (command == "Primary Monitor") { return new TargetPrimaryMonitorCommand(); }
            return null;
        }

        public string Command => "Primary Monitor";

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Screen(), " Primary Monitor");

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");
            state.FrozenScreen = Net.Surviveplus.LightCutter.Core.Screen.Freeze();

            var primary = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            var bounds = new Rectangle();
            foreach (var b in from s in System.Windows.Forms.Screen.AllScreens select s.Bounds)
            {
                bounds = Rectangle.Union(bounds, b);
            } // next b

            primary.Offset(bounds.Left * -1, bounds.Top * -1);

            state.CroppedImage = state.FrozenScreen?.Crop(primary);
        }
    }
}
