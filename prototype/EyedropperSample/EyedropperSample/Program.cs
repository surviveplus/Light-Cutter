using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyedropperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var threshold = 2000;

            foreach (var item in args)
            {
                var file = new FileInfo(item);
                if(file.Exists)
                {
                    Console.Write( $"{file.Name} ... ");
                    try
                    {
                        var textFile = Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(file.Name) + ".txt" );
                        var text = new StringBuilder();
                        using (var image = Bitmap.FromFile(file.FullName) )
                        using (var bitmap = new Bitmap(image) )
                        {

                            var colors = new Dictionary<Color, ColorCount>();

                            // TODO: unsafe
                            for (int x = 0; x < bitmap.Width; x++)
                            {
                                for (int y = 0; y < bitmap.Height; y++)
                                {
                                     var c = bitmap.GetPixel(x, y);
                                    if (colors.ContainsKey(c))
                                    {
                                        colors[c].Count += 1;
                                    }
                                    else
                                    {
                                        colors.Add(c, new ColorCount {Count = 1, X = x, Y = y });
                                    }
                                }
                            }

                            text.AppendLine("HTML\tR\tG\tB\tH\tS\tV");
                            foreach (var c in (from c in colors where c.Value.Count > threshold orderby c.Value.Y , c.Value.X  select c.Key))
                            {
                                text.AppendLine( $"{ColorTranslator.ToHtml(c)}\t{c.R}\t{c.G}\t{c.B}\t{c.GetHue()}\t{c.GetSaturation()}\t{c.GetBrightness()}");
                            } // next kvp

                            text.AppendLine();

                        } // end using (bitmap)
                        File.WriteAllText( textFile, text.ToString());

                        Console.WriteLine("done.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR. {ex.Message}");
                        Debug.WriteLine($"{file.Name} - ERROR.");
                        Debug.WriteLine(ex.ToString());
                    } // end try


                } // end if
            } // next item


        } // end sub


        private class ColorCount
        {
            public int Count { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

    } // end class
} // end namespace
