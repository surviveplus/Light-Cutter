using Net.Surviveplus.LightCutter.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands.Cutting
{
    public class LastRangeCommand : IActionCommand
    {
        public static LastRangeCommand FromCommand(string command)
        {
            if (command == "Last Range") { return new LastRangeCommand(); }
            return null;
        }
        public string Command => "Last Range";

        public bool IsEnabled => LastRange.HasValue;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.LastArea(), " Cut the same range");

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;
            if (!LastRange.HasValue) throw new InvalidOperationException("Last range is nothing.");

            Func<CroppedImage> crop;

            if (state.CroppedImage != null)
            {
                crop = () => state.CroppedImage?.Crop(LastRange.Value);

            }
            else
            {
                if (state.FrozenScreen == null)
                {
                    Debug.WriteLine($"{this.Command} - state.FrozenScreen and state.CroppedImage are null");
                    throw new InvalidOperationException("state.FrozenScreen and state.CroppedImage are null");
                } // end if

                crop = () => state.FrozenScreen?.Crop(LastRange.Value);
            } // end if

            state.CroppedImage = crop();

        } // end sub

        public static System.Drawing.Rectangle? LastRange = null;
    }
}
