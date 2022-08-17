namespace Load.Stub.Rest.dotNet.Effects
{
    public interface ISystemOperations
    {
        Task<IOperationRuntime> CreateBackgroundOperation(CancellationToken cancellationToken);
        Task<IOperationRuntime> CreateEndpoint(string endpoint,CancellationToken cancellationToken);
    }
}