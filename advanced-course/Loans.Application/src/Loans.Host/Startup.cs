using DCS.DecisionMakerService.Client.Kafka.Events;
using DCS.Platform.Kafka.Abstractions.Helpers;
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Loans.Host.Middleware;
using Loans.AppServices;
using Loans.AppServices.Limits;
using Loans.AppServices.Producer;
using Loans.DataAccess.Infrastructure;
using Loans.Host.Kafka;
using Serilog;

namespace Loans.Host;

public class Startup
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configuration">IConfiguration instance</param>
    /// <param name="webHostEnvironment">WebHostEnvironment instance</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        WebHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    ///     Configuration of web application
    /// </summary>
    private IConfiguration Configuration { get; set; }

    /// <summary>
    ///     Application web host environment
    /// </summary>
    private IWebHostEnvironment WebHostEnvironment { get; }

    /// <summary>
    ///     Метод, в котором происходит конфигурация сервисов приложения. Таких как логирование, 
    /// доступ к БД, и к другим инфраструктурным вещам
    /// </summary>
    /// <param name="services">Web app services collection</param>
    public void ConfigureServices(IServiceCollection services)
    {
        var kafkaOptions = Configuration.GetSection("Kafka").Get<KafkaOptions>();
        services.AddKafka(kafka => kafka
            .UseMicrosoftLog()
            .AddCluster(cluster => cluster
                .WithBrokers(kafkaOptions.Servers)
                .AddProducer<CalculateDecisionEventProducer>(builder =>
                {
                    builder.AddMiddlewares(middlewares => middlewares.AddSerializer<JsonCoreSerializer>());
                    builder.DefaultTopic(KafkaHelpers.GetTopic(typeof(CalculateDecisionEvent)));
                })
                .AddConsumer(consumer => consumer
                    .Topic(KafkaHelpers.GetTopic(typeof(CalculateDecisionEventResult)))
                    .WithGroupId(kafkaOptions.ConsumerGroup)
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSerializer<JsonCoreSerializer>()
                        .AddTypedHandlers(handlers => handlers
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<DecisionHandler>()
                            .WhenNoHandlerFound(context =>
                                Console.WriteLine("Message not handled > Partition: {0} | Offset: {1}",
                                    context.ConsumerContext.Partition,
                                    context.ConsumerContext.Offset)
                            )
                        )
                    )
                )
            )
        );
        services.AddScoped<ICalculateDecisionEventProducer, CalculateDecisionEventProducer>();
        services.AddScoped<DecisionHandler>();
        services.AddAppServices();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            
            loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
            
            loggingBuilder.AddSerilog();
        });
        services.AddDataAccess(Configuration);
        services.Configure<LoanApplicationLimitsOptions>(Configuration.GetSection("LoanConfiguration"));
        services.Configure<ClientLimitsOptions>(Configuration.GetSection("ClientConfiguration"));
        services.Configure<DecisionMakerOptions>(Configuration.GetSection("DecisionMakerService"));
        services.AddControllers(o => o.Filters.Add<ApplicationServicesExceptionsFilter>());
        services.AddSwaggerGen();
    }

    /// <summary>
    /// Этот метод вызывается во время работы приложения (в рантайме) 
    /// Здесь происходит конфигурация пайплайна обработки входящих запросов к серверу
    /// </summary>
    /// <param name="app">IApplication Builder object</param>
    /// <param name="env">IWwbHostEnvironment object</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        var kafkaBus = app.ApplicationServices.CreateKafkaBus();
        lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        // Добавление мидлваров в пайплайн обработки запросов
        app.UseMiddleware<ServiceNameMiddleware>();
        app.UseRouting(); // Добавляем Middleware для маршрутизации
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
