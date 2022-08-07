using Load.Stub.Rest.dotNet.Effects;
using Load.Stub.Rest.dotNet.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace load_stub_rest_dotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoController : ControllerBase
    {
        private readonly ILogger<DoController> _logger;
        private readonly IReadOnlyDictionary<OperationEffectType,IOperationEffect> _effects;

        public DoController(ILogger<DoController> logger, EffectsProvider effectsProvider)
        {
            _logger = logger;
            _effects = effectsProvider.Effects;
        }

        /// <summary>
        /// Hello world doc title
        /// </summary>
        /// <param name="request">reqe==</param>
        /// 
        [HttpPost(Name = "Operation")]
        [SwaggerRequestExample(typeof(OperationRequest),typeof(OperationRequest))]
        public Task<IActionResult> Operation(OperationRequest request)
        {
            _logger.LogTrace("Exevuting operation id");
            try
            {
                foreach (var modifier in request.EffectModifiers??Array.Empty<EffectModifier>())
                {
                    _effects[modifier.EffectType].Execute(modifier);
                }
                
            }
            finally
            {
                foreach (var item in _effects.Values)
                {
                    item.Dispose();
                }
            }
            
            return Task.FromResult((IActionResult)Ok());
        }
    }
}