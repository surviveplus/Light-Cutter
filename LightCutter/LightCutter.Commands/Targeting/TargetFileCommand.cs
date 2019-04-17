using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetFileCommand : IActionCommand
    {
        public static TargetFileCommand FromCommand(string command)
        {
            if (command == "File") { return new TargetFileCommand(); }
            return null;
        }

        public string Command => "File";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.File(), " File Image");

        public bool IsEnabled => true;

        public bool MustUac => false;


        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            using (var d = new System.Windows.Forms.OpenFileDialog () )
            {
                var r  = d.ShowDialog();
                if(r== System.Windows.Forms.DialogResult.OK)
                {
                    var bitmap = new System.Drawing.Bitmap(d.FileName);
                    state.CroppedImage = new Core.CroppedImage(bitmap);
                }
                else
                {
                    state.IsCanceled = true;
                    Debug.WriteLine($"{this.Command} - Canceled");
                }
            }
                
        }
    }
}
