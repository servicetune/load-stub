using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class FixedValueEffectModifier : ApplySpecificEffectModifierBase
    {
        public override OperationModifierType ModifierType => OperationModifierType.FixedValue;

        public override double Apply(EffectModifier item, double value) => item.Values["fixed"];
    }
}