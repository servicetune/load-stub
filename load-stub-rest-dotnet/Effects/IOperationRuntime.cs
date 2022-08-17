using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IOperationRuntime : IDisposable
    {
        OperationType OperationType { get; }

        IOperationRuntime SetConfig(JsonElement config, ModifiersRuntimeDelegate modifiersRuntime);

        Task Execute(CancellationToken cancellationToken);

    }

    public interface IOperationRuntimeWithChildren 
    {
        IOperationRuntime Children(OperationScope[] scope);
    }
}
