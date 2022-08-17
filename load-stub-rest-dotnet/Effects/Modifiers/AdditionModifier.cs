using Load.Stub.Rest.dotNet.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public class AdditionModifier : ApplyEffectModifierBase<AdditionModifier.ModAddConfig>
    {
        public AdditionModifier(JsonElement config) : base(config)
        {
        }

        [DataContract(Name = "AddValueModifierConfig")]
        public class ModAddConfig
        {
            [Required]
            public double Value { get; set; }
        }
        
        public override double Apply(double value) => value + Config.Value;

    }
}