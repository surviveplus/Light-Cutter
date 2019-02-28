using Net.Surviveplus.LightCutter.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Desktop
{
    public class LightCutter
    {
        public static Dictionary<string, Commands.ActionCommands> ActionCommands ;

        public static void InitializeActionCommands()
        {
            LightCutter.ActionCommands = new Dictionary<string, Commands.ActionCommands>();

            Action<string> add = command =>
            {
                // command example: "Wait > Screen > Cut > Save"
                var action = Commands.ActionCommands.FromCommands(command);
                LightCutter.ActionCommands.Add(command, action);
            };

            add("Screen > Cut > Copy");
            add("Screen > Cut > Save");
            add("Screen > Last Range > Save");

            //add("Wait > Screen > Cut > Copy");
            add("Wait > Screen > Cut > Save");
            add("Wait > Screen > Last Range  > Save");

            add("Primary Monitor > Save");
            add("Wait > Primary Monitor > Save");
        }
    }
}
