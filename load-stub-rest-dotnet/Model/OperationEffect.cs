namespace Load.Stub.Rest.dotNet.Model
{
    public enum OperationEffectType
    {
        ConsumeCpu,
        MemoryAllocate,
        ExecuteHttpRequest
    }

    public enum OperationModifierType
    {
        FixedValue,
        Multiplier,
        Addition,
        SinWave,
        RandomRange,
    }
}
