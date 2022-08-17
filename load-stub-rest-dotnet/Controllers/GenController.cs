//using Load.Stub.Rest.dotNet.Model;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.AdditionModifier;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.FixedValueModifier;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.GausianRandomModifier;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.MultiplierModifier;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.RandomRangeModifier;
//using static Load.Stub.Rest.dotNet.Effects.Modifiers.SinWaveModifier;

//namespace Load.Stub.Rest.dotNet.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class GenController : ControllerBase
//    {
//        public GenController()
//        {

//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok(new OperationScope{
//                 Type = OperationType.OperationsList,
//                 Children = Enumerable.Range(0, random.Next(6)).Select(s => GenerateOperation(null)).ToArray()
//            });
//        }

//        static OperationType[] SupportedOpTypes = new OperationType[]
//        {
//            OperationType.Sleep,
//            OperationType.ConsumeCpu,
//            OperationType.MemoryAllocate,
//        };

//        static OperationModifierType[] SupportedModifierTypes = new OperationModifierType[]
//        {
//           OperationModifierType.Multiplier,
//           OperationModifierType.Addition,
//           OperationModifierType.SinWave,
//           OperationModifierType.RandomRange,
//           OperationModifierType.GausianRandom,
//        };

//        static IReadOnlyDictionary<OperationModifierType, Func<JsonElement>> modGenRnadomConfig = new Dictionary<OperationModifierType, Func<JsonElement>>
//        {
//            { OperationModifierType.Multiplier, () =>       JsonSerializer.SerializeToElement( new ModMultiplyConfig{Value = random.NextDouble() * 5 } )},
//            { OperationModifierType.Addition, () =>         JsonSerializer.SerializeToElement( new ModAddConfig{Value = random.NextDouble() * 5 } )},
//            { OperationModifierType.SinWave, () =>          JsonSerializer.SerializeToElement( new ModSinWaveConfig{ Amplitude = random.NextDouble() * 5 , Frequency = 1} )},
//            { OperationModifierType.RandomRange, () =>      JsonSerializer.SerializeToElement( new ModRandomConfig{} )},
//            { OperationModifierType.GausianRandom, () =>    JsonSerializer.SerializeToElement( new ModGauseRandConfig{Mean = random.NextDouble() * 5 , StandardDeviation = random.NextDouble() } )},
//        };

//        private static Random random = new Random();

//        private OperationScope GenerateOperation(OperationType? operationType)
//        {
//            return new OperationScope()
//            {
//                Type = operationType.HasValue ? operationType.Value : SupportedOpTypes[random.Next(SupportedOpTypes.Length)],
//                Modifiers = GenModifiers(random.Next(6)),
//            };
//        }

//        private EffectModifier[] GenModifiers(int count)
//        {
//            var result = Enumerable.Range(0, count).Select(i =>
//            {
//                var type = SupportedModifierTypes[random.Next(SupportedModifierTypes.Length)];
//                return new EffectModifier()
//                {
//                    ModifierType = type,
//                    Values = modGenRnadomConfig[type](),
//                };
//            }).ToList();

//            var fixedMod = new EffectModifier
//            {
//                ModifierType = OperationModifierType.FixedValue,
//                Values = JsonSerializer.SerializeToElement(new ModFixedConfig { Value = random.NextDouble() * 5 })
//            };

//            result.Insert(0,fixedMod);

//            return result.ToArray();
//        }
//    }
//}
