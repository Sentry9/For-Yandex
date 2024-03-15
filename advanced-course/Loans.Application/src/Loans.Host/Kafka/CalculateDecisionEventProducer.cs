using DCS.DecisionMakerService.Client.Kafka.Events;
using KafkaFlow;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Producer;

namespace Loans.Host.Kafka;

public class CalculateDecisionEventProducer : ICalculateDecisionEventProducer
{
    private readonly IMessageProducer<CalculateDecisionEventProducer> _producer;

    public CalculateDecisionEventProducer(IMessageProducer<CalculateDecisionEventProducer> producer)
    {
        _producer = producer;
    }

    public async Task SendCalculateDecisionEvent(LoanApplicationModel eventMessage)
    {
        var message = await Map(eventMessage);
        await _producer.ProduceAsync(
            messageKey: message.ApplicationId.ToString(),
            messageValue: message);
    }
        

    public async Task<CalculateDecisionEvent> Map(LoanApplicationModel model)
    {
        var eventMessage = new CalculateDecisionEvent()
        {
            ApplicationId = model.Id,
            ApplicationDate = model.CreationDate,
            CreditAmount = model.Amount,
            CreditLenMonth = model.TermInYears * 12,
            ClientId = model.ClientModel.Id,
            BirthDay = model.ClientModel.BirthDate,
            IncomeAmount = model.ClientModel.Salary
        };
        return eventMessage;
    }
}   