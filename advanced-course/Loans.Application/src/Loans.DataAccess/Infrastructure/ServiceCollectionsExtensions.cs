using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.DataAccess.IInMemoryStorage;
using Loans.DataAccess.InMemoryStorage;
using Loans.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loans.DataAccess.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
    }
}