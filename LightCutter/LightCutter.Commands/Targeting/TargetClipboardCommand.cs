using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetClipboardCommand : IActionCommand
    {
        public static TargetClipboardCommand FromCommand(string command)
        {
            if (command == "Clipboard") { return new TargetClipboardCommand(); }
            return null;
        }

        public string Command => "Clipboard";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Clipboard(), " Clipboard Image");

        public bool IsEnabled => true;

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");

            try
            {
                using (var image = System.Windows.Forms.Clipboard.GetImage())
                {
                    var bitmap = new System.Drawing.Bitmap(image);
                    state.CroppedImage = new Core.CroppedImage(bitmap);
                }
            }
            catch
            {
                throw new TargetNotFoundException("Target not found. Please launch this action while there is a image in clipboard.");
            }
        }
    }
}
