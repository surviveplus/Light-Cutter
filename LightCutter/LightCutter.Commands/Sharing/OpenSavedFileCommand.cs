using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Sharing
{
    public class OpenSavedFileCommand : IActionCommand
    {
        public static OpenSavedFileCommand FromCommand(string command)
        {
            if (command == "Open") { return new OpenSavedFileCommand(); }
            return null;
        }

        public string Command => "Open";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.App(), " Open Saved File");

        public bool IsEnabled => true;

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            if(state.SavedFile == null)
            {
                Debug.WriteLine($"{this.Command} state.SavedFile are null");
                if (state.CroppedImage == null)
                {
                    if (state.FrozenScreen == null)
                    {
                        throw new Targeting.TargetNotSelectedException();
                    } // end if

                    state.CroppedImage = state.FrozenScreen.Crop();
                } // end if

                Debug.WriteLine($"{this.Command} save temp file.");
                using (var bitmap = state.CroppedImage?.GetBitmap())
                {
                    var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
                    bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
                    outputFile.Refresh();
                    state.SavedFile = outputFile;
                } // end using (cropped, bitmap )

            } // end if

            Debug.WriteLine($"{this.Command}");
            var p = Process.Start(state.SavedFile.FullName);
        }
    }
}
