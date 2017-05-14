using System;

namespace OperationMessaging
{
    [Serializable]
    public sealed class OperationRequest
    {
        public OperationRequest()
        {
            Parameters = new object[] {};
        }

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public object[] Parameters { get; set; }
    }
}
