using System;

namespace OperationMessaging
{
    [Serializable]
    public sealed class OperationResponse
    {
        public bool Succes => StatusCode == OperationStatusCodes.OK;

        public OperationStatusCodes StatusCode { get; set; }

        public object Result { get; set; }

        public string SuccessMessage { get; set; }

        public string NonSuccessMessage { get; set; }
    }
}