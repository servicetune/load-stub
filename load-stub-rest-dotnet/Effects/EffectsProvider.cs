using Load.Stub.Rest.dotNet.Model;
using System.Diagnostics;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class EffectsProvider
    {
        IReadOnlyDictionary<OperationEffectType, IOperationEffect> _operationEffects;
        public EffectsProvider(IEnumerable<IOperationEffect> operationEffects)
        {
            _operationEffects = operationEffects.ToDictionary(e => e.OperationEffectType);
        }

        public IReadOnlyDictionary<OperationEffectType,IOperationEffect> Effects => _operationEffects;
    }

    public interface IOperationEffect : IDisposable
    {
        OperationEffectType OperationEffectType { get; }

        Task Execute(EffectModifier effectModifiers);

    }

    public class MemoryAllocateOperation : IOperationEffect
    {
        public OperationEffectType OperationEffectType => OperationEffectType.MemoryAllocate;

        private byte[]? memory = null;
        private readonly IApplyEffectsModifier applyEffectsModifier;

        public MemoryAllocateOperation(IApplyEffectsModifier applyEffectsModifier)
        {
            this.applyEffectsModifier = applyEffectsModifier;
        }

        public void Dispose()
        {
            var x = memory?[0];
            memory = null;
        }

        public Task Execute(EffectModifier effectModifier)
        {
            int blockSize = (int)applyEffectsModifier.For(effectModifier).Apply(1);
            memory = new byte[blockSize];
            
            return Task.CompletedTask;
        }
    }

    public class CpuConsumeOperation : IOperationEffect
    {
        private Random? _random;
        private readonly IApplyEffectsModifier applyEffectsModifier;

        public OperationEffectType OperationEffectType => OperationEffectType.ConsumeCpu;

        public CpuConsumeOperation(IApplyEffectsModifier applyEffectsModifier)
        {
            _random = new Random();
            this.applyEffectsModifier = applyEffectsModifier;
        }

        public void Dispose()
        {
            _random = null;
        }

        public Task Execute(EffectModifier effectModifier)
        {
            var sw = Stopwatch.StartNew();
            var x = (double?)0.0;
            var cputimeMs = applyEffectsModifier.For(effectModifier).Apply(1);

            while (sw.Elapsed.TotalMilliseconds < cputimeMs)
            {
                x =  _random?.Next()/ Math.PI;
            }

            return Task.CompletedTask;
        }
    }
}
