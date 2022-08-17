using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    public class SleepOperation : OperationRuntimeBase<string>
    {
        private readonly IUnitConverter<TimeSpan> unitConverter;

        public SleepOperation(IUnitsConverterFactory unitConverterFactory, Serilog.ILogger logger) :base(OperationType.Sleep,logger.ForContext<SleepOperation>())
        {
            this.unitConverter=(IUnitConverter<TimeSpan>)unitConverterFactory.GetConverter(OperationType.Sleep);
        }

        public override async Task Executetion(CancellationToken cancellationToken)
        {
            var x = unitConverter.Convert(Config, Value); 
            await Task.Delay(x);
        }

    }
}
