using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects
{
    public delegate double ModifiersRuntimeDelegate(double value);

    public interface IModifierFactory
    {
        ModifiersRuntimeDelegate FromConfig(IEnumerable<EffectModifier> modifier);
    }
}