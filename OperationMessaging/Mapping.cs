using System;
using System.Collections.Generic;
using System.Reflection;

namespace OperationMessaging
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
            //***** TODO:Cache match for execution;
            return Parser.ParseRouteInstance(relativePath).Succes;
        }

        public object Execute(string relativePath)
        {
            //***** TODO:Dispose all on service dispose;
            var parseResult = Parser.ParseRouteInstance(relativePath);
            if (!parseResult.Succes) return parseResult;

            //***** TODO:Shared instances;
            var variables = (Dictionary<string, string>)Parser.ParseRouteInstance(relativePath).Result;
            var obj = Activator.CreateInstance(Action.ReflectedType);
            var parameters = Action.GetParameters();
            var ps = new List<object>();
            foreach (var parameter in parameters)
                ps.Add(Convert.ChangeType(variables[parameter.Name], parameter.ParameterType));

            //***** TODO:Call dispose if necessary;
            var disposableMapping = Action.ReflectedType.GetInterface("IDisposable", true);
            

            return Action.Invoke(obj, ps.ToArray());
        }
    }
}
