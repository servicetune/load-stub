using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public abstract class ApplyEffectModifierBase: IApplyEffectModifier
    {
        public Dictionary<OperationModifierType, IApplySpecificEffectModifier> Appliers { get; }
        public ApplyEffectModifierBase(IEnumerable<IApplySpecificEffectModifier> appliers)
        {
            Appliers = appliers.ToDictionary(d => d.ModifierType);
        }

        public abstract double Apply(double value);
    }
}