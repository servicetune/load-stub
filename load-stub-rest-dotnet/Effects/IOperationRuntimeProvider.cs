using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IOperationRuntimeProvider
    {
        IOperationRuntime For(OperationScope operation);
    }
}