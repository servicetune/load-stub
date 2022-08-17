using Load.Stub.Rest.dotNet.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public class FixedValueModifier : ApplyEffectModifierBase<FixedValueModifier.ModFixedConfig>
    {
        public FixedValueModifier(JsonElement config) : base(config)
        {
        }

        [DataContract(Name = "FixedValueModifierConfig")]
        public class ModFixedConfig
        {
            /// <summary>
            /// Fixed value to return.
            /// </summary>
            [Required]
            public double Value { get; set; }
        }

        public override double Apply(double value) => base.Config.Value;
    }

}