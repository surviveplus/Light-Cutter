using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands.Sharing
{
    public class CopyCommand : IActionCommand
    {
        public static CopyCommand FromCommand(string command)
        {
            if (command == "Copy") { return new CopyCommand(); }
            return null;
        }

        public string Command => "Copy";

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Copy(), " Copy");

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            if (state.CroppedImage == null)
            {
                if (state.FrozenScreen == null)
                {
                    Debug.WriteLine($"{this.Command} state.CroppedImage and state.FrozenScreen are null");
                    throw new InvalidOperationException("state.CroppedImage and state.FrozenScreen are null");
                } // end if

                state.CroppedImage = state.FrozenScreen.Crop();
            } // end if

            Debug.WriteLine($"{this.Command}");
            using (var bitmap = state.CroppedImage?.GetBitmap())
            {
                System.Windows.Forms.Clipboard.SetImage(bitmap);
            } // end using (cropped, bitmap )
        }
    }
}