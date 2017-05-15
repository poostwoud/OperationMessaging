using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculation;

namespace OperationMessaging.Test
{
    [TestClass]
    public class OperationServiceTests
    {
        private const string KnownRelativePath = "/calculator/add/1/2";
        private const string UnknownRelativePath = "/this/path/is/unknown";

        [TestMethod]
        public void UnknownRelativePathInExecution()
        {
            var service = new Facade();
            service.MapCorrect();
            var result = service.Execute(UnknownRelativePath);
            Assert.IsFalse(result.Succes);
            Assert.AreEqual(OperationStatusCodes.NotFound, result.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullOrWhitespaceRouteInMapping()
        {
            var service = new Facade();
            service.MapNullOrWhitespaceRoute();
            var result = service.Execute(KnownRelativePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnknownOrNullActionInMapping()
        {
            var service = new Facade();
            service.MapNullOrUnknownAction();
            var result = service.Execute(KnownRelativePath);
        }
    }

    public sealed class Facade : OperationService
    {
        public void MapCorrect()
        {
            Routes.Clear();

            Routes.Add(new Route(
                routeTemplate: "/calculator/add/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Add")));

            Routes.Add(new Route(
                routeTemplate: "/calculator/subtract/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Subtract")));

            Routes.Add(new Route(
                routeTemplate: "/calculator/divide/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Divide")));
        }

        public void MapNullOrWhitespaceRoute()
        {
            Routes.Clear();

            Routes.Add(new Route(
                routeTemplate: null,
                action: typeof(Calculator).GetMethod("Add")));
        }

        public void MapNullOrUnknownAction()
        {
            Routes.Clear();

            Routes.Add(new Route(
                routeTemplate: "/calculator/multiply/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Multiply")));
        }
    }
}
