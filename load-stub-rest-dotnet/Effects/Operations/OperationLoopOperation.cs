namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    public class OperationLoopOperation : OperationRuntimeBase<object>
    {
        private IOperationRuntime[] operationRuntimes;


        public OperationLoopOperation(IOperationRuntime[] operationRuntimes, Serilog.ILogger logger) :base(Model.OperationType.OperationLoop,logger.ForContext<OperationLoopOperation>())
        {
            this.operationRuntimes = operationRuntimes;
        }

        public override async Task Executetion(CancellationToken cancellationToken)
        {
            for (int i = 0; i < Value; i++)
            {
                foreach (var item in operationRuntimes)
                {
                    await item.Execute(cancellationToken);
                }
            }
        }
    }
}