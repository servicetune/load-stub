using Swashbuckle.AspNetCore.Filters;

namespace Load.Stub.Rest.dotNet.Model
{
    /// <summary>
    /// A request for execution of an operation, with optional midifiers.
    /// </summary>
    public class OperationRequest : IExamplesProvider<OperationRequest>
    {
        public ICollection<EffectModifier>? EffectModifiers { get; set; }


        public OperationRequest()
        {
            
        }

        public OperationRequest GetExamples()
        {
            return new OperationRequest()
            {
                EffectModifiers = new List<EffectModifier>()
                {
                    new EffectModifier()
                    {
                        EffectType = OperationEffectType.MemoryAllocate,
                        ModifierType = OperationModifierType.FixedValue,
                        Values = new Dictionary<string, double>()
                        {
                            ["fixed"]= 1
                        }
                    },
                    new EffectModifier()
                    {
                        EffectType = OperationEffectType.ConsumeCpu,
                        ModifierType = OperationModifierType.FixedValue,
                        Values = new Dictionary<string, double>()
                        {
                            ["fixed"]= 1
                        }
                    },
                }
            };
        }
    }
}
