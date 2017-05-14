using System;
using OperationMessaging;
using System.Collections.Generic;

namespace Calculation
{
    public sealed class Service : OperationService
    {
        public Service()
        {
            Map.Add(new Mapping(
                route: "calculator/add/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Add", new Type[] { typeof(int), typeof(int) })));

            Map.Add(new Mapping(
                route: "calculator/divide/{operand1}/{operand2}",
                action: typeof(Calculator).GetMethod("Divide", new Type[] { typeof(float), typeof(float) })));
        }
    }
}
