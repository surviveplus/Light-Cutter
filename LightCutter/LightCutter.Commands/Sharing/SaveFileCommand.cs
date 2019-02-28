using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands.Sharing
{
    public class SaveFileCommand : IActionCommand
    {
        public static SaveFileCommand FromCommand(string command)
        {
            if (command == "Save") { return new SaveFileCommand(); }
            return null;
        }

        public string Command => "Save";

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.Save(), " Save");

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
                var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
            } // end using (cropped, bitmap )
        }
    }
}