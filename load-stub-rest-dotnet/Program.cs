using System.Diagnostics;
using Load.Stub.Rest.dotNet;
using Serilog;

using var file = File.CreateText("Serilog.Diagnostics.log");
Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder
    .ConfigureInfraServices()
    .ConfigureApplicationServices()
    .Configuration();

var app = builder
          .Build();

try
{
    await app
        .ApplicationSetup()
        .RunAsync();
}
finally
{
    await app.DisposeAsync();

    Log.CloseAndFlush();

}


