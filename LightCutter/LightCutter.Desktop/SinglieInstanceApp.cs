using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;

namespace Net.Surviveplus.LightCutter.Desktop
{
    public class SinglieInstanceApp : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var single = new SinglieInstanceApp();
            single.Run(args);
        }

        public SinglieInstanceApp()
        {
            this.IsSingleInstance = true;
        }

        private App app;

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            this.app = new App();
            this.app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            this.app.OnStartupNextInstance(eventArgs);
        }
    }
}
