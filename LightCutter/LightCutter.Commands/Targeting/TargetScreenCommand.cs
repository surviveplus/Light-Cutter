using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetScreenCommand : IActionCommand
    {
        public static TargetScreenCommand FromCommand(string command)
        {
            if (command == "Screen") { return new TargetScreenCommand(); }
            return null;
        }

        public string Command => "Screen";

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");
            state.FrozenScreen = Net.Surviveplus.LightCutter.Core.Screen.Freeze();

        }
    }
}
