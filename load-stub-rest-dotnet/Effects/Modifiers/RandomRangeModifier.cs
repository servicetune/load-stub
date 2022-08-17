using Load.Stub.Rest.dotNet.Model;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public class RandomRangeModifier : ApplyEffectModifierBase<RandomRangeModifier.ModRandomConfig>
    {
        
        private Random random;

        public RandomRangeModifier(JsonElement jsonElement):base(jsonElement)
        {
            random = new Random();
        }

        [DataContract(Name = "RandomValueModifierConfig")]
        public class ModRandomConfig { }

        
        public override double Apply(double value) => value * random.NextDouble();

    }
}