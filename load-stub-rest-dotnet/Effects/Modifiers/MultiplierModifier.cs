using Load.Stub.Rest.dotNet.Model;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public partial class MultiplierModifier : ApplyEffectModifierBase<MultiplierModifier.ModMultiplyConfig>
    {
        public MultiplierModifier(JsonElement config) : base(config)
        {
        }

        public override double Apply(double value) => value * Config.Value;

    }
}