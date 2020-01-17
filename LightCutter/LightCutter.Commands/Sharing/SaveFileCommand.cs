using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Environment;

namespace Net.Surviveplus.LightCutter.Commands.Sharing
{
    public class SaveFileCommand : IActionCommand
    {

        #region IActionCommand members

        public string Command{
            get
            {
                if(this.Folder != null){ 
                    return $"Save ( \"{this.Folder.FullName}\" )";
                }else{
                    return "Save";
                }
            } // end get
        } // end property

        public bool IsEnabled => true;

        IEnumerable<object> IActionCommand.DisplayCommand
        {
            get{
                if (this.Folder != null)
                {
                    return ActionCommandDisplay.Create(new UI.Parts.Save(), $" Save ( ... {this.Folder.Name} )");
                }
                else
                {
                    return ActionCommandDisplay.Create(new UI.Parts.Save(), " Save");
                }

            }
        }

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            if (state.CroppedImage == null)
            {
                if (state.FrozenScreen == null)
                {
                    throw new Targeting.TargetNotSelectedException();
                } // end if

                state.CroppedImage = state.FrozenScreen.Crop();
            } // end if

            Debug.WriteLine($"{this.Command}");
            using (var bitmap = state.CroppedImage?.GetBitmap())
            {
                var outputFile = new System.IO.FileInfo(System.IO.Path.Combine(this.GetFolder().FullName, System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
                if(! outputFile.Directory.Exists)
                {
                    outputFile.Directory.Create();
                }
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);

                outputFile.Refresh();
                state.SavedFile = outputFile;
            } // end using (cropped, bitmap )
        }

        #endregion


        private static System.Text.RegularExpressions.Regex valueOfRegexCommand;

        private static System.Text.RegularExpressions.Regex RegexCommand
        {
            get
            {
                if (SaveFileCommand.valueOfRegexCommand == null) {
                    SaveFileCommand.valueOfRegexCommand = new System.Text.RegularExpressions.Regex("Save *\\( *\"(?<folder>.+)\" *\\)");
                }
                return SaveFileCommand.valueOfRegexCommand;
            }
        }

        public static SaveFileCommand FromCommand(string command)
        {
            if (command.StartsWith("Save"))
            {
                if (command == "Save") { return new SaveFileCommand(); }
                else
                {
                    var m = SaveFileCommand.RegexCommand.Match(command);
                    if (m.Success)
                    {
                        var folder = m.Groups["folder"].Value;
                        return new SaveFileCommand { Folder = new System.IO.DirectoryInfo(folder) };
                    } // end if
                } // endif                
            } // end if
            
            return null;
        } // end function

        public DirectoryInfo Folder { get; set; }

        public DirectoryInfo GetFolder()
        {
            if(this.Folder != null)
            {
                return this.Folder;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Settings.Default.DefaultFolder))
                {
                    return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                }
                else
                {
                    var specialFolder = GetSupportedSpecialFolder(Settings.Default.DefaultFolder);
                    if (specialFolder!=null)
                    {
                        return specialFolder;
                    }
                    else { 
                        return new DirectoryInfo(Settings.Default.DefaultFolder);
                    } // end if

                }
            }
        } // end function

        public static DirectoryInfo GetSupportedSpecialFolder(string text)
        {
            var supprotedSpecialFolders = new[] { SpecialFolder.Desktop, SpecialFolder.MyDocuments, SpecialFolder.MyPictures, SpecialFolder.MyVideos };

            var specialFolder =
                (from s in supprotedSpecialFolders
                 where string.Compare(s.ToString(), text, true) == 0
                 select new DirectoryInfo(Environment.GetFolderPath(s))
                 ).FirstOrDefault();

            return specialFolder;
        }

    } // end class
} // end namespace