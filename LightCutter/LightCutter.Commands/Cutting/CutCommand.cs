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
    public class CutCommand : IActionCommand
    {
        public static CutCommand FromCommand(string command)
        {
            if (command == "Cut") { return new CutCommand(); }
            return null;
        }
        public string Command => "Cut";

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create( new UI.Parts.Cutter() , " Cut");

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Net.Surviveplus.LightCutter.UI.FullScreenWindow w = null;
            Func<bool?> show;
            Func<CroppedImage> crop;

            if (state.CroppedImage != null)
            {
                show = () => w.ShowCroppedImage(state.CroppedImage);
                crop = () => state.CroppedImage?.Crop(w.CroppedBounds);

            }
            else {
                if (state.FrozenScreen == null)
                {
                    throw new Targeting.TargetNotSelectedException();
                } // end if

                show = () => w.ShowFrozenScreen(state.FrozenScreen);
                crop = () => state.FrozenScreen?.Crop(w.CroppedBounds);
            } // end if

            Debug.WriteLine($"{this.Command} - Start");
            w = new Net.Surviveplus.LightCutter.UI.FullScreenWindow(Settings.Default.GuideBackgroundTransparent, Settings.Default.GridPixel);
            var r = show();
            if (r.HasValue && r.Value)
            {
                Debug.WriteLine($"{this.Command} - Done");
                LastRangeCommand.LastRange = w.CroppedBounds;
                state.CroppedImage = crop();
            }
            else
            {
                state.IsCanceled = true;
                Debug.WriteLine($"{this.Command} - Canceled");

            } // end if
        }

    }
}
