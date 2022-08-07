using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class RandomRangeEffectModifier: ApplySpecificEffectModifierBase
    {
        private Random random;

        public RandomRangeEffectModifier()
        {
            random = new Random();
        }
        
        public override OperationModifierType ModifierType => OperationModifierType.RandomRange;
        public override double Apply(EffectModifier item, double value) => value * random.NextDouble();
    }
}