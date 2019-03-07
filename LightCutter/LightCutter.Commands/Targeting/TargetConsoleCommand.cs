using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetConsoleCommand : IActionCommand
    {
        public static TargetConsoleCommand FromCommand(string command)
        {
            if (command == "Console") { return new TargetConsoleCommand(); }
            return null;
        }

        public string Command => "Console";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.PowerShell(), " Console of PowerShell or Command Prompt");

        public bool IsEnabled => true;

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");

            try
            {
                //var bitmap = PrintWindowImage.PrintWindow("{ConsoleWindowClass}", "(Text Area)");
                var bitmap = PrintWindowImage.SaveByPrintWindowInside("{ConsoleWindowClass}");
                state.CroppedImage = new Core.CroppedImage(bitmap);
            }
            catch (TargetNotFoundException)
            {
                state.IsCanceled = true;
                // TODO: error messages
            }

        }
    }
}