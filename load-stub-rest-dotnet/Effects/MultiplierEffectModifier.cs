using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class MultiplierEffectModifier : ApplySpecificEffectModifierBase
    {
        public override OperationModifierType ModifierType => OperationModifierType.Multiplier;

        public override double Apply(EffectModifier item, double value) => value * item.Values["delta"];
    }
}