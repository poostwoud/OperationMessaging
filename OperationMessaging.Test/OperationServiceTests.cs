using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculation;

namespace OperationMessaging.Test
{
    [TestClass]
    public class OperationServiceTests
    {
        [TestMethod]
        public void UnknownRelativePath()
        {
            var service = new Service();
            var relativePath = "this/path/is/unknown";
            var result = service.Execute(relativePath);
            Assert.IsFalse(result.Succes);
            Assert.AreEqual("Unknown path", result.NonSuccessMessage);
        }

        [TestMethod]
        public void CalculatorAdd()
        {
            var service = new Service();
            var relativePath = "calculator/add/6/2";
            var result = service.Execute(relativePath);
            Assert.IsTrue(result.Succes);
            Assert.AreEqual(8, result.Result);
        }

        [TestMethod]
        public void CalculatorDivide1()
        {
            var service = new Service();
            var relativePath = "calculator/divide/6/2";
            var result = service.Execute(relativePath);
            Assert.IsTrue(result.Succes);
            Assert.AreEqual((float) 3, result.Result);
        }

        [TestMethod]
        public void CalculatorDivide2()
        {
            var service = new Service();
            var relativePath = "calculator/divide/5/2";
            var result = service.Execute(relativePath);
            Assert.IsTrue(result.Succes);
            Assert.AreEqual((float)2.5, result.Result);
        }
    }
}
