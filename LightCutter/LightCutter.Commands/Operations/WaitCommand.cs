using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Timer(), $" Wait {this.GetWaitTime().TotalSeconds}s");

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;
            System.Threading.Thread.Sleep(this.GetWaitTime());

            Debug.WriteLine($"{this.Command}");
        }

        public TimeSpan? WaitTime { get; set; }

        public bool MustUac => false;

        public TimeSpan GetWaitTime()
        {
            if (this.WaitTime.HasValue)
            {
                return this.WaitTime.Value;
            }
            else
            {
                return TimeSpan.FromSeconds(Settings.Default.DefaultWaitTimeSeconds);
            }

        }

    }
}
