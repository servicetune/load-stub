using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Model
{
    /// <summary>
    /// A specific operation scope
    /// </summary>
    public class OperationScope
    {
        /// <summary>
        /// Operation type
        /// </summary>
        [Required]
        public OperationType Type { get; set; }

        public JsonElement Config { get; set; }
        
        [Required]
        public EffectModifier[] Modifiers { get; set; } = new EffectModifier[0];

        public OperationScope[] Children { get; set; } = new OperationScope[0];
    }
    
}
