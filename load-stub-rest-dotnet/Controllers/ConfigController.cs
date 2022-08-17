using Load.Stub.Rest.dotNet.Configuration;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Load.Stub.Rest.dotNet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ICurrentConfig<AppStubOptions> monitor;
        private readonly Serilog.ILogger logger;

        public ConfigController(ICurrentConfig<AppStubOptions> monitor,Serilog.ILogger logger)
        {
            this.monitor = monitor;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get() => monitor.Value == null?NotFound("No config avalible."):Ok(monitor.Value);

        [HttpPost]
        public void Post([FromBody] AppStubOptions value)
        {
            logger.Warning("ConfigController.Post new options");
            monitor.SetConfig(value);
        }
    }
}
