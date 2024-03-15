using DCS.DecisionMakerService.Client.Kafka.Events;
using Loans.AppServices.Contracts.Models;

namespace Loans.AppServices.DecisionMakerService;

internal interface IDecisionMakerService
{
    Task CalculateDecision(LoanApplicationModel? query, CancellationToken cancellationToken);
}