using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class AdditionEffectModifier : ApplySpecificEffectModifierBase
    {
        public override OperationModifierType ModifierType => OperationModifierType.Addition;
        public override double Apply(EffectModifier item, double value) => value + item.Values["delta"];
    }
}