using System.Text.Json.Serialization;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Model.Example
{
    public class OperationRequestSample : Swashbuckle.AspNetCore.Filters.IExamplesProvider<OperationScope>
    {

        public OperationScope GetExamples()
        {
            return new OperationScope
            {
                Type = OperationType.ConsumeCpu,
                Modifiers = new EffectModifier[]
                {
                            new EffectModifier
                            {
                                ModifierType = OperationModifierType.FixedValue,
                                Values = JsonSerializer.SerializeToElement( new Effects.Modifiers.FixedValueModifier.ModFixedConfig
                                {
                                    Value = new Random().Next(1, 100)
                                })
                            }
                }
            };
        }
    }
}
