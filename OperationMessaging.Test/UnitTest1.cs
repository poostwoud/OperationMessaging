using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculation;

namespace OperationMessaging.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new Service();
            var relativePath = "calculator/add/1/2";
            var result = service.Execute(relativePath);
            Assert.AreEqual(3, result.Result);
        }
    }
}
