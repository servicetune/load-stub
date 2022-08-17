using Load.Stub.Rest.dotNet.Effects;
using Load.Stub.Rest.dotNet.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Load.Stub.Rest.dotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoController : ControllerBase
    {
        private readonly ILogger<DoController> _logger;
        private readonly IOperationRuntimeProvider _operationRuntimeProvider;

        public DoController(ILogger<DoController> logger, IOperationRuntimeProvider operationRuntimeProvider)
        {
            _logger = logger;
            _operationRuntimeProvider = operationRuntimeProvider;
        }

        /// <summary>
        /// Hello world doc title
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="cancellationToken"></param>
        /// 
        [HttpPost(Name = "Operation")]
        [SwaggerRequestExample(typeof(OperationScope), typeof(OperationScope))]
        public async Task<IActionResult> Operation(OperationScope  scope,CancellationToken cancellationToken)
        {
            using var ops = _operationRuntimeProvider.For(scope);
            
            _logger.LogTrace("Executing operation id");

            await ops.Execute(cancellationToken);
            
            return Ok();
        }
    }
}