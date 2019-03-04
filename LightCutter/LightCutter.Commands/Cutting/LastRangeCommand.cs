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

            if (state.FrozenScreen == null)
            {
                Debug.WriteLine($"{this.Command} - state.FrozenScreen is null");
                throw new InvalidOperationException("state.FrozenScreen is null");
            } // end if

            //if(state.CroppedImage != null)
            //{
            //    // TODO : w.ShowCroppedImage(state.CroppedImage );
            //}

            if (LastRange.HasValue)
            {
                using (var cropped = state.FrozenScreen?.Crop(LastRange.Value))
                using (var bitmap = cropped?.GetBitmap())
                {
                    var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
                    bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
                } // end using (cropped, bitmap )
            } // end if
        }

        public static System.Drawing.Rectangle? LastRange = null;
    }
}
