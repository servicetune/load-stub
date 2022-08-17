using Load.Stub.Rest.dotNet.Effects.Operations;
using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class OperationRuntimeProvider : IOperationRuntimeProvider
    {
        private readonly IModifierFactory modifierFactory;
        private readonly IServiceProvider serviceProvider;

        public OperationRuntimeProvider(IModifierFactory modifierFactory, IServiceProvider serviceProvider)
        {
            this.serviceProvider=serviceProvider;
            this.modifierFactory = modifierFactory;
        }
        public IOperationRuntime For(OperationScope operation)
        {
            var config = operation.Config;
            var modifiersRuntime = modifierFactory.FromConfig(operation.Modifiers);
            switch (operation.Type)
            {
                case OperationType.Sleep:

                    return serviceProvider.GetRequiredService<SleepOperation>().SetConfig(config, modifiersRuntime);
                case OperationType.ConsumeCpu:
                    return serviceProvider.GetRequiredService<CpuConsumeOperation>().SetConfig(config, modifiersRuntime);
                case OperationType.MemoryAllocate:
                    return serviceProvider.GetRequiredService<MemoryAllocateOperation>().SetConfig(config, modifiersRuntime);
                case OperationType.OperationsList:
                    return serviceProvider.GetRequiredService<OperationsListOperation>().Children(operation.Children).SetConfig(config, modifiersRuntime);
                //case OperationType.OperationLoop:
                //return new OperationLoopOperation(children.Select(c => For(c)).ToArray());
                case OperationType.ExecuteHttpRequest:
                    return serviceProvider.GetRequiredService<ExecuteHttpRequestOperation>();
                default:
                    throw new NotImplementedException($"Operation {operation} is not implemented");
            }
        }


    }
}