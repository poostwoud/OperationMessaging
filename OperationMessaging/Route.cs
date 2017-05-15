using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OperationMessaging
{
    public sealed class Route
    {
        public Route(string routeTemplate, MethodInfo action)
        {
            //*****
            if (string.IsNullOrWhiteSpace(routeTemplate)) throw new ArgumentNullException(nameof(routeTemplate));
            if (action == null) throw new ArgumentNullException(nameof(action));

            //*****
            RouteTemplate = routeTemplate;
            Parser = new RouteParser(routeTemplate);
            Parser.ParseRouteFormat();
            Action = action;
        }

        public string RouteTemplate { get; }

        public RouteParser Parser { get; }

        public MethodInfo Action { get; }

        public OperationResponse Execute(string relativePath)
        {
            //*****
            if (Action == null) return new OperationResponse { StatusCode = OperationStatusCodes.InternalServerError, NonSuccessMessage = "Action not set" };
            if (Action.ReflectedType == null) return new OperationResponse { StatusCode = OperationStatusCodes.InternalServerError, NonSuccessMessage = "Type not set" };
            var reflectedType = Action.ReflectedType;

            //*****
            var variables = Parser.ParseRouteInstance(relativePath);
            if (variables == null)
                return new OperationResponse
                {
                    StatusCode = OperationStatusCodes.NotFound
                };

            //***** TODO:Shared instances;
            var obj = Activator.CreateInstance(reflectedType);
            var parameters = Action.GetParameters();

            //***** Get result;
            var response = new OperationResponse();
            try
            {
                var result = Action.Invoke(obj, parameters.Select(parameter => Convert.ChangeType(variables[parameter.Name], parameter.ParameterType)).ToArray());
                response.StatusCode = OperationStatusCodes.OK;
                response.Result = result;
            }
            catch (Exception ex)
            {
                response.StatusCode = OperationStatusCodes.InternalServerError;
                response.NonSuccessMessage = ex.Message;
            }
            finally
            {
                //*****
                var disposeMethod = reflectedType.GetMethod("Dispose");
                if (disposeMethod == null)
                    // ReSharper disable RedundantAssignment
                    obj = null;
                    // ReSharper restore RedundantAssignment
                else
                    disposeMethod.Invoke(obj, new object[] {});
            }

            //*****
            return response;
        }
    }
}
