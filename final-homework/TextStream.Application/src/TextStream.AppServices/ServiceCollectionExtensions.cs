using Microsoft.Extensions.DependencyInjection;
using TextStream.AppServices.Contracts.Handlers;
using TextStream.AppServices.Contracts.Validators;
using TextStream.AppServices.Handlers;
using TextStream.AppServices.Validators;

namespace TextStream.AppServices;

public static class ServiceCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IHandler, Handler>();
        services.AddScoped<IBroadcastValidator, BroadcastValidator>();
    }
}