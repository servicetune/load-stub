using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    internal class OperationsListOperation : IOperationRuntimeWithChildren 
    {
        private readonly IOperationRuntimeProvider operationRuntimeProvider;

        public OperationsListOperation(IOperationRuntimeProvider operationRuntimeProvider)
        {
            this.operationRuntimeProvider=operationRuntimeProvider;
        }
        

        public IOperationRuntime Children(OperationScope[] scope)
        {
            return new OperationsListRuntime(scope.Select(c => operationRuntimeProvider.For(c)).ToArray());
        }
    }
}