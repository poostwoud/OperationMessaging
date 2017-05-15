namespace OperationMessaging
{
    public interface IOperationService
    {
        OperationResponse Execute(string relativePath, string data = null);
    }
}