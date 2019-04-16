using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetVirtualMachineConnectionCommand : IActionCommand
    {
        public static TargetVirtualMachineConnectionCommand FromCommand(string command)
        {
            if (command == "Virtual Machine") { return new TargetVirtualMachineConnectionCommand(); }
            return null;
        }

        public string Command => "Virtual Machine";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.VirtualMachine(), " Virtual Machine");

        public bool IsEnabled => true;

        public bool MustUac => true;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");

            var bitmap = PrintWindowImage.PrintWindow("<vmconnect>", "{UIMainClass}" , "Virtual Machine Connection");
            state.CroppedImage = new Core.CroppedImage(bitmap);

        }
    }
}