using DCS.DecisionMakerService.Client.Kafka.Events;
using Loans.AppServices.Contracts.Models;

namespace Loans.AppServices.Producer;

public interface ICalculateDecisionEventProducer
{
    Task SendCalculateDecisionEvent(LoanApplicationModel eventMessage);
}