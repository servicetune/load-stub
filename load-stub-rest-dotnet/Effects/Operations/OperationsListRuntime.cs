using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    internal class OperationsListRuntime : IOperationRuntime
    {
        private IOperationRuntime[] operationRuntimes;

        public OperationsListRuntime(IOperationRuntime[] operationRuntimes)
        {
            this.operationRuntimes=operationRuntimes;
        }

        public OperationType OperationType => throw new NotImplementedException();

        public void Dispose()
        {
            foreach (var operation in operationRuntimes)
            {
                operation.Dispose();
            }
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            foreach (var operation in operationRuntimes)
            {
                await operation.Execute(cancellationToken);
            }
        }

        public IOperationRuntime SetConfig(JsonElement config, ModifiersRuntimeDelegate modifiersRuntime)
        {
            return this;
        }
    }
}