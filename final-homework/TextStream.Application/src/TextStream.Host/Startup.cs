using Serilog;
using TextStream.AppServices;
using TextStream.DataAccess.Infrastructure;
using TextStream.Host.Hub;
using TextStream.Host.Middleware;

namespace TextStream.Host;

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
        services.AddSignalR();
        services.AddAppServices();
        services.AddDataAccess(Configuration); ;
        services.AddControllers();
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
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        // Добавление мидлваров в пайплайн обработки запросов
        app.UseMiddleware<ServiceNameMiddleware>();
        app.UseRouting(); // Добавляем Middleware для маршрутизации
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<BroadcastHub>("/broadcast");
        });
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
