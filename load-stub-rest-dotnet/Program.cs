using Load.Stub.Rest.dotNet.Effects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Prometheus;
using Prometheus.DotNetRuntime;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddTransient<IOperationEffect, MemoryAllocateOperation>();
builder.Services.AddTransient<IOperationEffect, CpuConsumeOperation>();
builder.Services.AddTransient<EffectsProvider>();
builder.Services.AddTransient<IApplyEffectsModifier, ApplyEffectModifier>();

builder.Services.AddTransient<IApplySpecificEffectModifier, FixedValueEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, MultiplierEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, AdditionEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, RandomRangeEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, SinWaveEffectModifier>();

var metrics = new KestrelMetricServer(port: 9090).Start();
var diagnostocPrometheus = DiagnosticSourceAdapter.StartListening();
var evenyCounterAdapter = EventCounterAdapter.StartListening();

IDisposable statsCollector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

builder.Services.AddHealthChecks();

builder.Services.AddOpenTracing(otb =>
{
    otb.AddAspNetCore(g =>
    {
        g.StartRootSpans = true;
    })
    .AddHttpHandler(g =>
    {
        g.StartRootSpans = true;
    })
    .AddGenericDiagnostics();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamples();
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGen((opt) =>
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var fvi = FileVersionInfo.GetVersionInfo(assembly.Location)?.FileVersion;
        string version = fvi ?? "0.0-unknown";
        opt.ExampleFilters();
        opt.EnableAnnotations();
        opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "load-stub-rest-dotnet.xml"));
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = $"{assembly.GetName().Name} API", Version = $"v{version}" });
        opt.UseAllOfForInheritance();
        opt.UseAllOfToExtendReferenceSchemas();
        opt.UseOneOfForPolymorphism();

        var xmlFilename = $"{assembly.GetName().Name}.xml";
        opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });


builder.Services.AddHttpClient(Options.DefaultName)
        .UseHttpClientMetrics();

builder.Services.AddHttpLogging(g =>
{

});

builder.Host.ConfigureHostOptions(ho =>
{
    ho.ShutdownTimeout = TimeSpan.FromSeconds(20);
    ho.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();

app.UseHealthChecks("/health");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


//HttpClientMetricsExtensions.UseHttpClientMetrics(app);
app.UseMetricServer();
app.UseHttpMetrics();
app.MapMetrics("/metrics");
app.UseRouting();
app.UseEndpoints(
    endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapMetrics();
    }
);

app.MapControllers();

try
{
    await app.RunAsync();
}
finally
{
    // Stops listening for DiagnosticSource events.
    diagnostocPrometheus.Dispose();
    // Stops listening for EventCounter events.
    evenyCounterAdapter.Dispose();

    statsCollector.Dispose();

    await metrics.StopAsync();
}
