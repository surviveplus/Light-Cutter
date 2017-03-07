using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Surviveplus.CodedUIQuery;
using System.Windows.Automation;
using System.Diagnostics;

namespace Net.Surviveplus.LightCutter
{
    public static class Resizer
    {

        public static async Task ResizeAsync()
        {
            await Task.Run(() =>
            {

                // Find Remote Desktop window
                var windows = Desktop.Elements.WaitForChildren("{TscShellContainerClass}", TimeSpan.FromSeconds(3));
                foreach (var w in windows)
                {

                    var outBounds = (w as WpfElement)?.UIAutomationElement.Current.BoundingRectangle;
                    var inBounds = (w.Find("{UIMainClass}").FirstOrDefault() as WpfElement)?.UIAutomationElement.Current.BoundingRectangle;
                    var virtualBounds = (w.Find("{OPWindowClass}").FirstOrDefault() as WpfElement)?.UIAutomationElement.Current.BoundingRectangle;


                    Debug.WriteLine(outBounds);
                    Debug.WriteLine(inBounds);
                    Debug.WriteLine(virtualBounds);

                    double inWidth = inBounds.Value.Width;
                    double inHeight = virtualBounds.Value.Height * inWidth / virtualBounds.Value.Width;

                    double outWidth = inWidth + (outBounds.Value.Width - inBounds.Value.Width);
                    double outHeight = inHeight + (outBounds.Value.Height - inBounds.Value.Height);


                    var p = (w as WpfElement)?.UIAutomationElement.GetCurrentPattern(TransformPattern.Pattern) as TransformPattern;
                    p.Resize(outWidth, outHeight);
                } // next w

            });

        } // end function

    } // end class
} // end namespace
