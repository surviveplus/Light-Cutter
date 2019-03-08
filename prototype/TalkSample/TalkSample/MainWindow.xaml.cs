using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
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

namespace TalkSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // see: https://www.wpf-tutorial.com/audio-video/speech-synthesis-making-wpf-talk/
            this.s = new SpeechSynthesizer();

            //foreach (var item in s.GetInstalledVoices())
            //{
            //    Debug.WriteLine(item.VoiceInfo.Name);
            //}

            //s.SelectVoice("Microsoft Haruka Desktop");
            //s.SelectVoice("Microsoft Zira Desktop");

            var voice = (
                from v in s.GetInstalledVoices()
                where v.VoiceInfo.Culture.Name == "en-US"
                select v.VoiceInfo).FirstOrDefault();

            if (voice != null)
            {
                this.s.SelectVoice(voice.Name);
            }
            this.s.Rate = 2;

        }
        private SpeechSynthesizer s;

        private void TalkButtons_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var text = button.Tag as string;

            if (!string.IsNullOrWhiteSpace(text))
            {
                this.SayText(text);
            }
        }

        private void SayText(string text)
        {
            //this.s.Speak(text);
            this.s.SpeakAsync(text);
        }

        private async void CountDown_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => {
                var startTime = System.DateTime.Now;
                var endTime = startTime.AddSeconds(3);
                var totalSeconds = (endTime - startTime).TotalSeconds;

                //Debug.WriteLine(totalSeconds);
                SayText(totalSeconds.ToString());

                var lastCount = totalSeconds;
                while(System.DateTime.Now < endTime)
                {
                    var count = Math.Ceiling( (endTime - System.DateTime.Now).TotalSeconds );

                    if(lastCount > count)
                    {
                        lastCount = count;
                        //Debug.WriteLine(count);
                        SayText(count.ToString());
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(20));
                }

                //Debug.WriteLine("Start");
                SayText("Start");
            });

        }
    }
}


