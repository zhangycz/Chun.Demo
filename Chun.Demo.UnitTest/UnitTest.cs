using System;
using Chun.Demo.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chun.Demo.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestThread testThread = new TestThread();
            testThread.TestMain();
        }
    }
}
