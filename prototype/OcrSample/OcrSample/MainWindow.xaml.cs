using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OcrSample
{
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                var dialog = new OpenFileDialog();
                if( dialog.ShowDialog(this).Value )
                {

                    var text = new System.Text.StringBuilder();
                    button.IsEnabled = false;
                    {
                        var result = await OcrWrapper.RecognizeAsync(dialog.FileName);
                        foreach (var line in result.Lines)
                        {
                            text.AppendLine(line.Text);
                        }
                    }
                    this.resultBlock.Text = text.ToString();
                } // end if

            }
            finally
            {
                button.IsEnabled = true;
            }


        }
    }

#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
}
