using Load.Stub.Rest.dotNet.Effects;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddTransient<IOperationEffect, MemoryAllocateOperation>();
builder.Services.AddTransient<IOperationEffect, CpuConsumeOperation>();
builder.Services.AddTransient<EffectsProvider>();
builder.Services.AddTransient<IApplyEffectsModifier, ApplyEffectModifier>();

builder.Services.AddTransient<IApplySpecificEffectModifier, FixedValueEffectModifier >();
builder.Services.AddTransient<IApplySpecificEffectModifier, MultiplierEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, AdditionEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, RandomRangeEffectModifier>();
builder.Services.AddTransient<IApplySpecificEffectModifier, SinWaveEffectModifier>();


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

builder.Services.AddTransient<IHostedService, MyHostedService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
 
await app.RunAsync();


