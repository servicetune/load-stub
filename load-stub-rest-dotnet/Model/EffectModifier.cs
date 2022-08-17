using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Load.Stub.Rest.dotNet.Model
{

    /// <summary>
    /// Modifies an effect value
    /// </summary>
    public class EffectModifier
    {
        [Required]
        public OperationModifierType ModifierType { get; internal set; }

        [Required]
        public JsonElement Values { get; set; }
    }
}
