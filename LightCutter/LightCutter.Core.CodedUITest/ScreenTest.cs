using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using System.Linq;
using System.IO;

namespace LightCutter.Core.CodedUITest
{
    /// <summary>
    /// CodedUITest1 の概要の説明
    /// </summary>
    [CodedUITest]
    public class ScreenTest
    {
        public ScreenTest()
        {
        }

        [TestMethod]
        public void Freeze_Immediate_FullSize()
        {
            // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。

            var bounds = new Rectangle();
            foreach (var b in from s in Screen.AllScreens select s.Bounds){
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var startTime = DateTime.Now;

            // Start Test

            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze())
            using (var cropped = frozen?.Crop())
            using (var bitmap = cropped?.GetBitmap())
            {
                var finishTime = DateTime.Now;
                Assert.IsTrue(finishTime - startTime < TimeSpan.FromSeconds(1));

                Assert.IsNotNull(bitmap);
                Assert.AreEqual(bounds.Size, bitmap.Size);

                FileInfo outputFile = GetOutputFileName();
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);

                this.TestContext.AddResultFile(outputFile.FullName);
            }

        } // end function

        [TestMethod]
        public void Freeze_Immediate_Part()
        {
            // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。

            var bounds = new Rectangle();
            foreach (var b in from s in Screen.AllScreens select s.Bounds)
            {
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var part = new Rectangle(10, 10, 100, 100);
            var startTime = DateTime.Now;

            // Start Test

            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze())
            using (var cropped = frozen?.Crop(part))
            using (var bitmap = cropped?.GetBitmap())
            {
                var finishTime = DateTime.Now;
                Assert.IsTrue(finishTime - startTime < TimeSpan.FromSeconds(1));

                Assert.IsNotNull(bitmap);
                Assert.AreEqual(part.Size, bitmap.Size);

                FileInfo outputFile = GetOutputFileName();
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);

                this.TestContext.AddResultFile(outputFile.FullName);
            }

        } // end function

        [TestMethod]
        public void Freeze_AfterSpecifiedTime_FullSize()
        {
            // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。

            var bounds = new Rectangle();
            foreach (var b in from s in Screen.AllScreens select s.Bounds)
            {
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var startTime = DateTime.Now;

            // Start Test

            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze(DateTime.Now + TimeSpan.FromSeconds(3)))
            using (var cropped = frozen?.Crop())
            using (var bitmap = cropped?.GetBitmap())
            {
                var finishTime = DateTime.Now;
                Assert.IsTrue(finishTime - startTime > TimeSpan.FromSeconds(3));
                Assert.IsTrue(finishTime - startTime < TimeSpan.FromSeconds(4));

                Assert.IsNotNull(bitmap);
                Assert.AreEqual(bounds.Size, bitmap.Size);

                FileInfo outputFile = GetOutputFileName();
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);

                this.TestContext.AddResultFile(outputFile.FullName);
            }

        } // end function

        [TestMethod]
        public void Freeze_AfterSpecifiedTime_Part()
        {
            // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。

            var bounds = new Rectangle();
            foreach (var b in from s in Screen.AllScreens select s.Bounds)
            {
                bounds = Rectangle.Union(bounds, b);
            } // next b

            var part = new Rectangle(10, 10, 100, 100);
            var startTime = DateTime.Now;

            // Start Test

            using (var frozen = Net.Surviveplus.LightCutter.Core.Screen.Freeze( DateTime.Now + TimeSpan.FromSeconds(3)))
            using (var cropped = frozen?.Crop(part))
            using (var bitmap = cropped?.GetBitmap())
            {
                var finishTime = DateTime.Now;
                Assert.IsTrue(finishTime - startTime > TimeSpan.FromSeconds(3));
                Assert.IsTrue(finishTime - startTime < TimeSpan.FromSeconds(4));

                Assert.IsNotNull(bitmap);
                Assert.AreEqual(part.Size, bitmap.Size);

                FileInfo outputFile = GetOutputFileName();
                bitmap.Save(outputFile.FullName, System.Drawing.Imaging.ImageFormat.Png);

                this.TestContext.AddResultFile(outputFile.FullName);
            }

        } // end function

        private FileInfo GetOutputFileName()
        {
            var outputFile = new FileInfo(Path.Combine(this.TestContext.TestResultsDirectory, this.TestContext.TestName + " " + System.DateTime.Now.ToString("yyyyMMdd HHmmssfff", System.Threading.Thread.CurrentThread.CurrentUICulture) + ".png"));
            System.IO.Directory.CreateDirectory(outputFile.DirectoryName);
            return outputFile;
        }

        #region 追加のテスト属性

        // テストを作成する際には、次の追加属性を使用できます:

        ////各テストを実行する前に、TestInitialize を使用してコードを実行してください 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。
        //}

        ////各テストを実行した後に、TestCleanup を使用してコードを実行してください
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // このテストのコードを生成するには、ショートカット メニューの [コード化された UI テストのコードの生成] をクリックし、メニュー項目の 1 つをクリックします。
        //}

        #endregion

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;
    }
}
