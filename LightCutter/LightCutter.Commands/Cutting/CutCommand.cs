using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Cutting
{
    public class CutCommand : IActionCommand
    {
        public static CutCommand FromCommand(string command)
        {
            if (command == "Cut") { return new CutCommand(); }
            return null;
        }
        public string Command => "Cut";

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            if(state.FrozenScreen == null)
            {
                Debug.WriteLine($"{this.Command} - state.FrozenScreen is null");
                throw new InvalidOperationException("state.FrozenScreen is null");
            } // end if

            //if(state.CroppedImage != null)
            //{
            //    // TODO : w.ShowCroppedImage(state.CroppedImage );
            //}

            Debug.WriteLine($"{this.Command} - Start");
            var w = new Net.Surviveplus.LightCutter.UI.FullScreenWindow(Settings.Default.GuideBackgroundTransparent, Settings.Default.GridPixel);
            var r = w.ShowFrozenScreen(state.FrozenScreen);
            if (r.HasValue && r.Value)
            {
                Debug.WriteLine($"{this.Command} - Done");
                state.LastRange = w.CroppedBounds;

                state.CroppedImage = state.FrozenScreen?.Crop(w.CroppedBounds);
            }
            else
            {
                state.IsCanceled = true;
                Debug.WriteLine($"{this.Command} - Canceled");

            } // end if
        }

    }
}
