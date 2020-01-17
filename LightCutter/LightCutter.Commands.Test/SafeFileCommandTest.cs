using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.LightCutter.Commands.Sharing;

namespace LightCutter.Commands.Test
{
    [TestClass]
    public class SafeFileCommandTest
    {
        [TestMethod]
        public void Test_FromCommand()
        {

            var path = @"c:\folderName";
            var action = SaveFileCommand.FromCommand( $"Save ( \"{path}\" )");
            Assert.AreEqual(path, action.Folder.FullName);
        }
    }
}
