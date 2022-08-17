using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public partial class MultiplierModifier
    {
        [DataContract(Name = "MultiplyValueModifierConfig")]
        public class ModMultiplyConfig
        {
            [Required]
            public double Value { get; set; }
        }

    }
}