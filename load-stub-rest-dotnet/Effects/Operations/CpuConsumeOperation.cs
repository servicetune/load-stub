using Load.Stub.Rest.dotNet.Effects.Operations.Config;
using Load.Stub.Rest.dotNet.Model;
using System.Diagnostics;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    public class CpuConsumeOperation : OperationRuntimeBase<string>
    {
        private readonly IUnitConverter<TimeSpan> unitConverter;
        private Random _random;


        public CpuConsumeOperation(Serilog.ILogger logger,IUnitsConverterFactory unitConverterFactory) : base(OperationType.ConsumeCpu,logger.ForContext<CpuConsumeOperation>())
        {
            _random = new Random();
            unitConverter=(IUnitConverter<TimeSpan>)unitConverterFactory.GetConverter(OperationType.ConsumeCpu);
        }

        public override Task Executetion(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
#pragma warning disable CS8604 // Possible null reference argument.
            var duration = unitConverter.Convert(Config, base.Value);
#pragma warning restore CS8604 // Possible null reference argument.
            double x=1;
            while (sw.Elapsed < duration && !cancellationToken.IsCancellationRequested)
            {
                x = _random.Next() / Math.PI;
            }

            return Task.FromResult(x);
        }
    }
}
