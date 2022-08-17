//using Load.Stub.Rest.dotNet.Configuration;
//using Load.Stub.Rest.dotNet.Effects;
//using Load.Stub.Rest.dotNet.Model;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Filters;

//namespace Load.Stub.Rest.dotNet.Controllers
//{

//    [ApiController]
//	[Route("[controller]")]
//	public class DynamicController : ControllerBase
//    {
//        private readonly AppStubOptions? options;
//        private readonly ISystemOperations systemOperations;
//        private readonly Serilog.ILogger logger;

//        public DynamicController(ISystemOperations systemOperations, Serilog.ILogger logger)
//        {
//            this.logger = logger;
//        }

//        [HttpGet()]
//        public async Task<IActionResult> ListAllEnpoints()
//        {
//            return Ok(options?.Endpoints);
//        }
        

//		[HttpGet("run/{name}")]
//		public async Task<IActionResult> Get(string name,CancellationToken cancellationToken = default(CancellationToken))
//		{
//            if (!(options?.Endpoints?.TryGetValue(name, out var endpoint)??false))
//                return NotFound(name);


//            //foreach (var modifier in endpoint)
//            //{
//            //    cancellationToken.ThrowIfCancellationRequested();
//            //    await effectsProvider.Operations[modifier.Type].Execute(modifier.Modifiers);
//            //}

//            return Ok();
//        }
//	}
//}