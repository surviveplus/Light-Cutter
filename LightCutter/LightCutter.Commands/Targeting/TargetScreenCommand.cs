using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Screen(), " Stop Screen");

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");
            state.FrozenScreen = Net.Surviveplus.LightCutter.Core.Screen.Freeze();

        }
    }
}
