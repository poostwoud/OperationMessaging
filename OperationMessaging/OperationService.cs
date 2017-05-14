using System;

namespace OperationMessaging
{
    public class OperationService : MarshalByRefObject, IOperationService
    {
        public virtual OperationResponse Execute(string relativePath, string data = null)
        {
            throw new NotImplementedException();
        }
    }
}