using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    public class MemoryAllocateOperation : OperationRuntimeBase<object>
    {

        private byte[]? memory = null;

        public MemoryAllocateOperation(Serilog.ILogger logger) : base(OperationType.MemoryAllocate,logger.ForContext<MemoryAllocateOperation>()) { }

        public override void Dispose()
        {
            var x = memory?[0];
            memory = null;
        }

        public override Task Executetion(CancellationToken cancellationToken)
        {
            int blockSize = Math.Max(1,(int)Value);

            memory = new byte[blockSize];

            return Task.CompletedTask;
        }

    }
}
