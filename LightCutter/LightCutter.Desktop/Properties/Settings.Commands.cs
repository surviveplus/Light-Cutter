using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Desktop.Properties
{
    internal partial class Settings 
    {
        public void UpdateCommandsSettings()
        {
            var d = Net.Surviveplus.LightCutter.Commands.Settings.Default;

            d.GuideBackgroundTransparent = Settings.Default.GuideBackgroundTransparent;
            d.GridPixel= Settings.Default.GridPixel;
            d.DefaultWaitTimeSeconds = Settings.Default.DefaultWaitTimeSeconds;
            d.DefaultFolder = Settings.Default.DefaultFolder;
        }

        public void SaveAndUpdateCommandsSettings()
        {
            Settings.Default.Save();
            this.UpdateCommandsSettings();
        }

        public void UpgradeDefaultActionName()
        {
            // from Ver.7.0 to Ver.7.1
            switch (Settings.Default.DefaultActionName)
            {
                case "CutAndCopy":
                    Settings.Default.DefaultActionName = "Screen > Cut > Copy";
                    break;

                case "CutAndSave":
                    Settings.Default.DefaultActionName = "Screen > Cut > Save";
                    break;

                case "CutSameAreaAndSave":
                    Settings.Default.DefaultActionName = "Screen > Last Range > Save";
                    break;

                case "CountdownCutAndSave":
                    Settings.Default.DefaultActionName = "Wait > Screen > Cut > Save";
                    break;

                case "CountdownCutSaveAreaAndSave":
                    Settings.Default.DefaultActionName = "Wait > Screen > Last Range  > Save";
                    break;

                case "SavePrimaryMonitor":
                    Settings.Default.DefaultActionName = "Primary Monitor > Save";
                    break;

                case "CountdownSavePrimaryMonitor":
                    Settings.Default.DefaultActionName = "Wait > Primary Monitor > Save";
                    break;

                default:
                    break;
            }
        }
    }
}
