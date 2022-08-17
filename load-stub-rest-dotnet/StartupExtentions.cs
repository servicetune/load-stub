using Load.Stub.Rest.dotNet.Configuration;
using Load.Stub.Rest.dotNet.Effects;
using Load.Stub.Rest.dotNet.Effects.Operations;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Load.Stub.Rest.dotNet
{
    public static class StartupExtentions
    {
        public static WebApplicationBuilder ConfigureInfraServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks();

            builder.Services.AddHostedService<MetricServiceHost>();
            builder.Services.AddHttpClient(Options.DefaultName)
                    .UseHttpClientMetrics();

            //builder.Services.AddOpenTracing(otb =>
            //{
            //    otb.AddAspNetCore(g =>
            //    {
            //        g.StartRootSpans = true;
            //    })
            //    .AddHttpHandler(g =>
            //    {
            //        g.StartRootSpans = true;
            //    })
            //    .AddGenericDiagnostics();
            //});
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.AllowInputFormatterExceptionMessages = true;
                    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerExamples();
            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

            
            builder.Services.AddSwaggerGen((opt) =>
            {
                opt.SchemaFilter<ModifierFactory>();

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


            builder.Services.AddHttpLogging(a => { });

            builder.Host.ConfigureHostOptions(ho =>
            {
                ho.ShutdownTimeout = TimeSpan.FromSeconds(20);
                ho.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            builder.Host.UseSerilog((ctx, lcfg) =>
            {
                lcfg.ReadFrom.Configuration(ctx.Configuration);
            });

            return builder;
        }

        public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
        {
            //builder.Services.AddTransient<IOperation, MemoryAllocateOperation>();
            //builder.Services.AddTransient<IOperation, CpuConsumeOperation>();
            //builder.Services.AddTransient<IOperation, SleepOperation>();
            //builder.Services.AddTransient<IOperation, ExecuteHttpRequestOperation>();

            //builder.Services.AddTransient<EffectsProvider>();
            //builder.Services.AddTransient<IApplyEffects, ApplyEffectModifier>();

            //builder.Services.AddTransient<IApplySpecificEffectModifier, FixedValueEffectModifier>();
            //builder.Services.AddTransient<IApplySpecificEffectModifier, MultiplierEffectModifier>();
            //builder.Services.AddTransient<IApplySpecificEffectModifier, AdditionEffectModifier>();
            //builder.Services.AddTransient<IApplySpecificEffectModifier, RandomRangeEffectModifier>();
            //builder.Services.AddTransient<IApplySpecificEffectModifier, GausianRandomEffectModifier>();

            //builder.Services.AddTransient<IApplySpecificEffectModifier, SinWaveEffectModifier>();

            builder.Services.AddTransient<ISystemOperations, SystemOperations>();
            builder.Services.AddTransient<IOperationRuntimeProvider, OperationRuntimeProvider>();
            builder.Services.AddSingleton<IModifierFactory, ModifierFactory>();
            builder.Services.AddSingleton<IUnitsConverterFactory, UnitsConverterFactory>();

            builder.Services.AddTransient<OperationsListOperation>();
            builder.Services.AddTransient<CpuConsumeOperation>();
            builder.Services.AddTransient<SleepOperation>();
            builder.Services.AddTransient<MemoryAllocateOperation>();


            builder.Services.AddSingleton<ICurrentConfig<AppStubOptions>>(svc =>
            {
                var path = svc.GetRequiredService<IConfiguration>().GetValue<string>("StubConfigPath");
                return new JsonFileMonitor<AppStubOptions>(svc.GetRequiredService<Serilog.ILogger>() , path);
            });
            builder.Services.AddHostedService<AppStub>();

            return builder;
        }


        public static WebApplicationBuilder Configuration(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureAppConfiguration((HostBuilderContext hostingContext, IConfigurationBuilder config) =>
            {
                config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                config.AddEnvironmentVariables(prefix: "STUB_");

                var configPath = hostingContext.Configuration.GetValue<string>("SerilogConfigPath");
                if (!string.IsNullOrWhiteSpace(configPath))
                {
                    config.AddJsonFile(configPath, true, true);
                }
                else
                {
                    config.AddJsonFile("serilog.json", true, false);
                    config.AddJsonFile($"serilog.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, false);
                }

                var configs = hostingContext.Configuration.GetChildren().Where(c => c.Key.ToLower().Contains("serilog")).ToArray();
                var envs = Environment.GetEnvironmentVariables().Keys.Cast<string>().Where(s => s.ToLower().Contains("serilog"));

            });
            return builder;
        }

        public static WebApplication ApplicationSetup(this WebApplication app)
        {
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
            return app;
        }




    }
}
