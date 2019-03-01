using System;
using System.Diagnostics;
using Net.Surviveplus.LightCutter.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.LightCutter.Commands.Targeting;
using Net.Surviveplus.LightCutter.Commands.Operations;
using Net.Surviveplus.LightCutter.Commands.Cutting;
using Net.Surviveplus.LightCutter.Commands.Sharing;

namespace LightCutter.Commands.Test
{
    [TestClass]
    public class ActionCommandsTest
    {
        [TestMethod]
        public void Test_CutAndSave()
        {
            var action = new ActionCommands();
            action.Commands.Add(new WaitCommand());
            action.Commands.Add(new TargetScreenCommand());
            action.Commands.Add(new CutCommand());
            action.Commands.Add(new SaveFileCommand());

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

        [TestMethod]
        public void Test_CutAndSave_Text()
        {
            var action = ActionCommands.FromCommands("Wait > Screen > Cut > Save");

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }


        [TestMethod]
        public void Test_ScreenSave()
        {
            var action = new ActionCommands();
            action.Commands.Add(new TargetScreenCommand());
            action.Commands.Add(new SaveFileCommand());

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

        [TestMethod]
        public void Test_ScreenSave_Text()
        {
            var action = ActionCommands.FromCommands("Screen > Save");

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

        [TestMethod]
        public void Test_SavePrimaryMonitor()
        {
            var action = new ActionCommands();
            action.Commands.Add(new TargetPrimaryMonitorCommand());
            action.Commands.Add(new SaveFileCommand());

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

        [TestMethod]
        public void Test_SavePrimaryMonitor_Text()
        {
            var action = ActionCommands.FromCommands("Primary Monitor > Save");
            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

        [TestMethod]
        public void Test_Waite_Text()
        {
            var action = ActionCommands.FromCommands("Wait");

            Debug.WriteLine(action.ToString());
            Debug.WriteLine("");
            action.Do();
        }

    }
}
