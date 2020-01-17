using Net.Surviveplus.LightCutter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Cutting
{
    public class RangeCommand : IActionCommand
    {
        #region IActionCommand members
        public string Command => $"Range ({this.Bounds.Left.ToString()}, {this.Bounds.Top.ToString()}, {this.Bounds.Width.ToString()}, {this.Bounds.Height.ToString()})";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.LastArea(), $" Range ({this.Bounds.Left.ToString()}, {this.Bounds.Top.ToString()}, {this.Bounds.Width.ToString()}, {this.Bounds.Height.ToString()})");

        public bool IsEnabled => true;

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Func<CroppedImage> crop;

            if (state.CroppedImage != null)
            {
                crop = () => state.CroppedImage?.Crop(Bounds);

            }
            else
            {
                if (state.FrozenScreen == null)
                {
                    throw new Targeting.TargetNotSelectedException();
                } // end if

                crop = () => state.FrozenScreen?.Crop(Bounds);
            } // end if

            state.CroppedImage = crop();

        } // end sub
        #endregion

        private static System.Text.RegularExpressions.Regex valueOfRegexCommand;

        private static System.Text.RegularExpressions.Regex RegexCommand
        {
            get
            {
                if (RangeCommand.valueOfRegexCommand == null)
                {
                    RangeCommand.valueOfRegexCommand = new System.Text.RegularExpressions.Regex("Range *\\( *(?<left>\\d+), *(?<top>\\d+), *(?<width>\\d+), *(?<height>\\d+) *\\)");
                }
                return RangeCommand.valueOfRegexCommand;
            }
        }

        public System.Drawing.Rectangle Bounds { get; set; } = new System.Drawing.Rectangle(0, 0, 100, 100);

        public static RangeCommand FromCommand(string command)
        {
            if (command.StartsWith("Range"))
            {
                var m = RangeCommand.RegexCommand.Match(command);
                if (m.Success)
                {
                    try
                    {
                        var bounds = new System.Drawing.Rectangle(int.Parse(m.Groups["left"].Value), int.Parse(m.Groups["top"].Value), int.Parse(m.Groups["width"].Value), int.Parse(m.Groups["height"].Value));
                        return new RangeCommand { Bounds = bounds };
                    }
                    catch
                    {
                        return null;
                    }
                } // end if
            } // end if

            return null;


        } // end function

    } // end class
} // end namespace
