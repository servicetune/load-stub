using System.Text.Json.Serialization;

namespace Load.Stub.Rest.dotNet.Model
{
    public class EffectModifier
    {
        public OperationModifierType ModifierType { get; internal set; }
        public OperationEffectType EffectType { get; set;}
        public Dictionary<string,double> Values { get; set; }

        public EffectModifier()
        {
            Values = new Dictionary<string, double>();
        }
    }
}
