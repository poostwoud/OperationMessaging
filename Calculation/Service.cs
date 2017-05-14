using System;
using OperationMessaging;
using System.Collections.Generic;
using System.Reflection;

namespace Calculation
{
    public sealed class Mapping
    {
        public Mapping(string route, MethodInfo action)
        {
            Route = route;
            Parser = new RouteParser(route);
            Parser.ParseRouteFormat();
            Action = action;
        }

        public string Route { get; }

        public RouteParser Parser { get; }

        public MethodInfo Action { get; }

        public bool IsMatch(string relativePath)
        {
            return Parser.ParseRouteInstance(relativePath).Succes;
        }

        public object Execute(string relativePath)
        {
            var variables = (Dictionary<string, string>)Parser.ParseRouteInstance(relativePath).Result;
            var obj = Activator.CreateInstance(Action.ReflectedType);
            return Action.Invoke(obj, new object[] { 1, 2 });
        }
    }

    public sealed class Service : OperationService
    {
        public override OperationResponse Execute(string relativePath, string data = null)
        {
            var map = new List<Mapping>();

            var mapping = new Mapping(
                route: "calculator/add/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Add", new Type[] { typeof(int), typeof(int) })
            );

            map.Add(mapping);

            //*****
            foreach (var item in map)
                if (item.IsMatch(relativePath))
                    return new OperationResponse
                    {
                        Succes = true,
                        Result = item.Execute(relativePath)
                    };

            return new OperationResponse
            {
                Succes = false,
                Result = null,
                NonSuccessMessage = "Unknown relative path"
            };
        }
    }
}
