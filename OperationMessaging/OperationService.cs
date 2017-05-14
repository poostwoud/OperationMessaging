using System;
using System.Collections.Generic;

namespace OperationMessaging
{
    public class OperationService : MarshalByRefObject, IOperationService
    {
        protected OperationService()
        {
            Map = new List<Mapping>();
        }
        
        protected List<Mapping> Map { get; }

        public OperationResponse Execute(string relativePath, string data = null)
        {
            //*****
            foreach (var item in Map)
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
                NonSuccessMessage = "Unknown path"
            };
        }
    }
}