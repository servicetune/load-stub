using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IUnitsConverterFactory
    {
        IUnitsConvert GetConverter(OperationType operation);
    }

    public class UnitsConverterFactory : IUnitsConverterFactory
    {
        public IUnitsConvert GetConverter(OperationType operation)
        {
            switch (operation)
            {
                case OperationType.Sleep:
                case OperationType.ConsumeCpu:
                case OperationType.OperationLoop:
                    return new TimeUnitConverter();
                case OperationType.MemoryAllocate:
                    return new BytesUnitConverter();
                
                case OperationType.ExecuteHttpRequest:
                case OperationType.OperationsList:
                        return new NullConverter();
                default:
                    break;
            }

            throw new NotSupportedException($"Operation {operation} is not supported");
        }
    }
}