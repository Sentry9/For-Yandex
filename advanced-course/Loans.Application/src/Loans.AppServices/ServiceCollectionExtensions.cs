using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.DecisionMakerService;
using Loans.AppServices.Handlers;
using Loans.AppServices.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Loans.AppServices;

public static class ServiceCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IClientHandler, ClientHandler>();
        services.AddHttpClient<DecisionMakerService.DecisionMakerService>();
        services.AddScoped<ILoanValidator, LoanValidator>();
        services.AddScoped<IDecisionMakerService, DecisionMakerService.DecisionMakerService>();
        services.AddScoped<IClientValidator, ClientValidator>();
        services.AddScoped<ILoanHandler, LoanHandler>();
    }
}