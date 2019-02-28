using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Operations
{
    public class WaitCommand : IActionCommand
    {
        public static WaitCommand FromCommand(string command)
        {
            if (command == "Wait") { return new WaitCommand(); }
            return null;
        }

        public string Command => "Wait";

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            // TODO : wait 3s etc..

            Debug.WriteLine($"{this.Command}");
        }
    }
}
