using Load.Stub.Rest.dotNet.Effects.Modifiers;
using Load.Stub.Rest.dotNet.Model;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class ModifierFactory : IModifierFactory, ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(EffectModifier))
            {
                return;
            }
            else
            {
                var oneOfs = new List<OpenApiSchema>()
                {
                    context.SchemaGenerator.GenerateSchema(typeof(FixedValueModifier.ModFixedConfig),context.SchemaRepository),
                    context.SchemaGenerator.GenerateSchema(typeof(MultiplierModifier.ModMultiplyConfig),context.SchemaRepository),
                    context.SchemaGenerator.GenerateSchema(typeof(AdditionModifier.ModAddConfig),context.SchemaRepository),
                    context.SchemaGenerator.GenerateSchema(typeof(SinWaveModifier.ModSinWaveConfig),context.SchemaRepository),
                    context.SchemaGenerator.GenerateSchema(typeof(GausianRandomModifier.ModGauseRandConfig),context.SchemaRepository),
                    context.SchemaGenerator.GenerateSchema(typeof(RandomRangeModifier.ModRandomConfig),context.SchemaRepository),
                };
                var values = schema.Properties["values"];
                values.Type = "object";
                values.OneOf = oneOfs;
            }


        }

        public ModifiersRuntimeDelegate FromConfig(IEnumerable<EffectModifier> modifier)
        {
            var modifiers = modifier.Select(m => CreateModifier(m.ModifierType, m.Values)).ToArray();
            return (double source) => modifiers.Aggregate<IModifierRuntime, double>(source, (s, m) => m.Apply(s));
        }

        private IModifierRuntime CreateModifier(OperationModifierType modifierType, JsonElement element)
        {
            switch (modifierType)
            {
                case OperationModifierType.FixedValue:
                    return new FixedValueModifier(element);
                case OperationModifierType.Multiplier:
                    return new MultiplierModifier(element);
                case OperationModifierType.Addition:
                    return new AdditionModifier(element);
                case OperationModifierType.SinWave:
                    return new SinWaveModifier(element);
                case OperationModifierType.RandomRange:
                    return new RandomRangeModifier(element);
                case OperationModifierType.GausianRandom:
                    return new GausianRandomModifier(element);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}