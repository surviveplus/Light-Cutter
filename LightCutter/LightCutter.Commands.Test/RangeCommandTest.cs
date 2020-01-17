using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.LightCutter.Commands.Cutting;

namespace LightCutter.Commands.Test
{
    [TestClass]
    public class RangeCommandTest
    {
        [TestMethod]
        public void Test_FromCommand()
        {
            var action = RangeCommand.FromCommand("Range ( 10, 20, 100, 200 )");
            Assert.AreEqual(new System.Drawing.Rectangle(10, 20, 100, 200), action.Bounds);
        }
    }
}
