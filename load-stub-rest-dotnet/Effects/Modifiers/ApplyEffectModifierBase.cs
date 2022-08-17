using Load.Stub.Rest.dotNet.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Load.Stub.Rest.dotNet.Effects.Modifiers
{
    public abstract class ApplyEffectModifierBase<TConfig> : IModifierRuntime
        where TConfig : class
    {
        private readonly TConfig config;

        protected TConfig Config => config;

        public ApplyEffectModifierBase(JsonElement config)
        {

            var json = config.GetRawText();
            if (json == null)
            {
                throw new ValidationException("Config is null");
            }

            var obj = JsonSerializer.Deserialize<TConfig>(json);
            if (obj == null)
                throw new ArgumentException("Desirialisation failed.",nameof(config));

            this.config = obj;
            
            if (this.config == null)
                throw new ValidationException("Config is null");

        }

        public abstract double Apply(double value);
    }
}