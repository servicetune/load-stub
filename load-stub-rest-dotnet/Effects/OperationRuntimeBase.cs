using Load.Stub.Rest.dotNet.Effects.Operations;
using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects
{
    public abstract class OperationRuntimeBase<TConfig> : IOperationRuntime where TConfig : class
    {

        public OperationType OperationType { get; }
        public Serilog.ILogger Logger { get; }
        protected ModifiersRuntimeDelegate? Modifiers { get;set; }

        public TConfig? Config { get; private set; }

        protected double Value { get; private set; }

        public OperationRuntimeBase(OperationType operationType,Serilog.ILogger logger)
        {
            OperationType = operationType;
            Logger=logger;
            Value = 0;
            Modifiers = null;
            Config = null;
        }


        public virtual void Dispose()
        {

        }

        public abstract Task Executetion(CancellationToken cancellationToken);

        public virtual Task Execute(CancellationToken cancellationToken)
        {
            if (Config == null || Modifiers == null)
            {
                throw new InvalidOperationException("Config not set");
            }

            Value = Modifiers(Value);
            this.Logger.Debug("Executing operation {opType}. Calculated Value {value}", GetType().Name, Value);
            return Executetion(cancellationToken);
        }

        public IOperationRuntime SetConfig(JsonElement config, ModifiersRuntimeDelegate modifiersRuntime)
        {
            try
            {
                var val = config.Deserialize<TConfig>(new JsonSerializerOptions { });
                if (val == null)
                {
                    throw new InvalidDataException($"Failed to desirialize ");
                }
                Config = val;
                Modifiers = modifiersRuntime;
                Logger.Debug("Operation {opType}. Config and modifiers set. Runtime ready.", GetType().Name);
                return this;
            }
            catch (JsonException jsonexception)
            {
                Logger.Error("Failed to conver '{config}' into {type}", config.GetRawText(),typeof(TConfig).Name);
                throw;
            }            
        }

    }
}
