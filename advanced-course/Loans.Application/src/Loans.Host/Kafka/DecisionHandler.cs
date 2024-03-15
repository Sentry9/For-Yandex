using DCS.DecisionMakerService.Client.Kafka.Enums;
using DCS.DecisionMakerService.Client.Kafka.Events;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Loans.Api.Contracts.Status;
using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Models;

namespace Loans.Host.Kafka
{
    public class DecisionHandler : IMessageHandler<CalculateDecisionEventResult>
    {
        private readonly ILoanHandler _loanHandler;

        public DecisionHandler(ILoanHandler loanHandler)
        {
            _loanHandler = loanHandler;
        }

        public async Task Handle(IMessageContext context, CalculateDecisionEventResult message)
        {
            var businessLogicRequest = Map(message);
            await _loanHandler.SetLoanStatus(businessLogicRequest);
        }

        private static LoanApplicationModel Map(CalculateDecisionEventResult message)
        {
            var model = new LoanApplicationModel()
            {
                Id = message.ApplicationId
            };
            switch (message.Decision.DecisionStatus)
            {
                case DecisionStatus.Approval:
                    model.Status = LoansStatus.Approved;
                    break;
                case DecisionStatus.Refuse:
                    model.Status = LoansStatus.Denied;
                    break;
                case DecisionStatus.Unknown:
                    model.Status = LoansStatus.Unknown;
                    break;
                default:
                    model.Status = LoansStatus.InProgress;
                    break;
            }
            return model;
        }
    }
}