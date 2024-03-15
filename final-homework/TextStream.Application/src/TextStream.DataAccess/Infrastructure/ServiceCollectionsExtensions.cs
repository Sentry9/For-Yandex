using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextStream.AppServices.Contracts.RepositoryInterfaces;
using TextStream.DataAccess.Repository;

namespace TextStream.DataAccess.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IBroadcastRepository, BroadcastRepository>();
    }
}