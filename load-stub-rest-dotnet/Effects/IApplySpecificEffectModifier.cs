using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IApplySpecificEffectModifier 
    {
        OperationModifierType ModifierType { get; }

        double Apply(EffectModifier item, double value);
    }
}