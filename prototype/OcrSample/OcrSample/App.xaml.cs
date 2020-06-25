using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OcrSample
{
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            int count = 0;
            foreach (var item in e.Args)
            {
                var f = new System.IO.FileInfo(item);
                if (f.Exists)
                {
                    var newFile = System.IO.Path.Combine(f.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(f.FullName) + ".txt");

                    var text = new System.Text.StringBuilder();
                    var result = await OcrWrapper.RecognizeAsync(f.FullName);
                    foreach (var line in result.Lines)
                    {
                        text.AppendLine(line.Text);
                    }
                    System.IO.File.WriteAllText(newFile, text.ToString());
                    count += 1;
                }
            }

            if (count > 0)
            {
                MessageBox.Show($"{count} 件のファイルを保存しました。");
                App.Current.Shutdown();
            }
        }
    }
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

}
