using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.LightCutter.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Core.Tests
{
    [TestClass()]
    public class FrozenScreenTests
    {
        [TestMethod()]
        public void CropTest()
        {
            var bound = new Rectangle(0,0, 10,10);

            using (var frozen = new FrozenScreen(bound))
            {
            }

        }
    }
}