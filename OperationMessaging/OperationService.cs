using System;
using System.Collections.Generic;

namespace OperationMessaging
{
    public class OperationService : MarshalByRefObject, IOperationService
    {
        protected OperationService()
        {
            Routes = new List<Route>();
        }
        
        protected List<Route> Routes { get; }

        public OperationResponse Execute(string relativePath, string data = null)
        {
            //***** TODO:Support data;
            //*****
            foreach (var item in Routes)
            {
                var result = item.Execute(relativePath);
                if (result.StatusCode == OperationStatusCodes.NotFound) continue;
                return result;
            }

            //*****
            return new OperationResponse
            {
                StatusCode = OperationStatusCodes.NotFound,
                Result = null,
                NonSuccessMessage = "Unknown path"
            };
        }
    }
}