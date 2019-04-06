using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands
{
    public class Settings
    {
        public static Settings Default { get; private set; } = new Settings();

        public bool GuideBackgroundTransparent { get; set; } = true;
        public int GridPixel { get; set; } = 16;
        public int DefaultWaitTimeSeconds { get; set; } = 3;

        public string DefaultFolder { get; set; } = "Desktop";
    }
}
