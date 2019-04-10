using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.UI
{
    public class WindowsTheme : ViewModelBase
    {
        public static WindowsTheme GetUserTheme()
        {
            try
            {
                var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                return new WindowsTheme { Theme = (WindowsThemes)reg.GetValue("AppsUseLightTheme") };
            }
            catch
            {
                return new WindowsTheme();
            }
        } // end function

        public WindowsTheme()
        {
            this.Theme = WindowsThemes.Light;
        }

        private WindowsThemes valueOfTheme;

        public WindowsThemes Theme
        {
            get
            {
                return this.valueOfTheme;
            }
            set
            {
                this.SetProperty(ref this.valueOfTheme, value);
            }
        }

        public static WindowsTheme Current { get; set; } = WindowsTheme.GetUserTheme();

    } // end class
   

    public enum WindowsThemes : int
    {
        Dark = 0,
        Light = 1,
    }


}
