using MathNet.Numerics.Distributions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public class GausianRandomModifier : ApplyEffectModifierBase<GausianRandomModifier.ModGauseRandConfig>
    {
        [DataContract(Name = "GausianRandomModifierConfig")]
        public class ModGauseRandConfig
        {
            [Required]
            public double Mean { get; set; }

            [Required]
            public double StandardDeviation { get; set; }
        }

        public GausianRandomModifier(JsonElement element) : base(element)
        {

        }
        
        public override double Apply(double value)
        {
            var normalDist = new Normal(Config.Mean, Config.StandardDeviation);

            double randomGaussianValue = normalDist.Sample();

            return randomGaussianValue;
        }
    }
}