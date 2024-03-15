using DCS.DecisionMakerService.Client.Kafka.Events;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Producer;

namespace Loans.AppServices.DecisionMakerService
{
    public class DecisionMakerService : IDecisionMakerService
    {
        private readonly ICalculateDecisionEventProducer _kafkaProducer;

        public DecisionMakerService(ICalculateDecisionEventProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }
        
        public async Task CalculateDecision(LoanApplicationModel? query, CancellationToken cancellationToken)
        {
            await _kafkaProducer.SendCalculateDecisionEvent(query);
        }
    }
}