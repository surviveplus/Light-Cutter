using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Editing
{
    public class TrimColorCommand : IActionCommand
    {
        public static TrimColorCommand FromCommand(string command)
        {
            if (command == "Trim Color") { return new TrimColorCommand(); }
            return null;
        }

        public string Command => "Trim Color";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.LastArea(), " Trim by Color");

        public bool IsEnabled => true;

        public bool MustUac => false;

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
                System.Drawing.Color? lastColor = null;
                bool exitForFor = false;

                Action<int, int> checkColor = (i, j) =>
                 {
                     var color = bitmap.GetPixel(i, j);

                     if (!lastColor.HasValue)
                     {
                         lastColor = color;
                     }
                     else
                     {
                         exitForFor = lastColor != color;
                     }
                 };

                // left -> right
                int left = 0;
                exitForFor = false;
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        checkColor(i, j);
                        if (exitForFor) break;
                    }
                    if (exitForFor) break;
                    left = i;
                }

                // top -> bottom
                int top = 0;
                exitForFor = false;
                for (int j = 0; j < bitmap.Height; j++)
                {
                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        checkColor(i, j);
                        if (exitForFor) break;
                    }
                    if (exitForFor) break;
                    top = j;
                }

                // left <-  right
                int right = 0;
                exitForFor = false;
                for (int i = bitmap.Width-1; 0<= i ; i--)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        checkColor(i, j);
                        if (exitForFor) break;
                    }
                    if (exitForFor) break;
                    right = i;
                }

                // top <- bottom
                int bottom = 0;
                exitForFor = false;
                for (int j = bitmap.Height-1; 0 <=j ; j--)
                {
                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        checkColor(i, j);
                        if (exitForFor) break;
                    }
                    if (exitForFor) break;
                    bottom = j;
                }

                // new 
                var bounds = new System.Drawing.Rectangle(
                    left,
                    top,
                    right - left, 
                    bottom - top);

                var newBitmap = new System.Drawing.Bitmap(bounds.Width, bounds.Height);


                using (var g = System.Drawing.Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(bitmap, 0,0,bounds, System.Drawing.GraphicsUnit.Pixel);
                }

                state.CroppedImage.Dispose();
                state.CroppedImage = new Core.CroppedImage(newBitmap);

            } // end using (cropped, bitmap )

        }
    }
}
