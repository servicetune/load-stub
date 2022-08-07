using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IApplyEffectModifier
    {
        double Apply(double value);
    }

    public abstract class ApplySpecificEffectModifierBase : IApplySpecificEffectModifier
    {
        public abstract OperationModifierType ModifierType { get; }

        public ApplySpecificEffectModifierBase()
        {
        }

        public abstract double Apply(EffectModifier item, double value);
    }

    public class SinWaveEffectModifier : ApplySpecificEffectModifierBase
    {
        public override OperationModifierType ModifierType => OperationModifierType.SinWave;

        public override double Apply(EffectModifier item, double value) => value + item.Values["amplitude"] * Math.Sin(item.Values["frequency"] * value);

    }

    public interface IApplyEffectsModifier:IApplyEffectModifier
    {
        IApplyEffectModifier For(EffectModifier effectModifier);
    }

    public class ApplyEffectModifier : ApplyEffectModifierBase, IApplyEffectsModifier
    {
        public List<EffectModifier> Modifiers { get; }

        public ApplyEffectModifier(IEnumerable<IApplySpecificEffectModifier> appliers):base(appliers)
        {
            Modifiers = new List<EffectModifier>();
        }


        public override double Apply(double value)
        {
            foreach (var item in Modifiers)
            {
                value = Appliers[item.ModifierType].Apply(item,value); 
            }
            return value;
        }

        public IApplyEffectModifier For(EffectModifier effectModifier)
        {
            Modifiers.Add(effectModifier);
            return this;
        }
    }
}