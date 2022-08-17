using Load.Stub.Rest.dotNet.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public class SinWaveModifier : ApplyEffectModifierBase<SinWaveModifier.ModSinWaveConfig>
    {
        private double offset;

        public class ModSinWaveConfig
        {
            [Required]
            public double Amplitude { get; set; }
            [Required]
            public double Frequency { get; set; }
        }

        public SinWaveModifier(System.Text.Json.JsonElement json) : base(json)
        {
            offset = 0.0d;
        }

        public override double Apply(double value)
        {
            return value + Config.Amplitude  * Math.Sin(offset += Config.Frequency);
        }
    }
}