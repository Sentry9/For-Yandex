using Loans.Api.Contracts.Api;
using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Responses;
using Loans.AppServices.Contracts.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Host.Controllers
{
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase, IClientController
    {
        private readonly IClientHandler _clientHandler;
        private IClientController _clientControllerImplementation;

        public ClientController(IClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
        }

        [HttpGet("{clientId}/loans")]
        public async Task<List<LoanIdResponse>> GetContracts(long clientId, CancellationToken cancellationToken)
        {
            List<LoanIdResponse> contracts = await _clientHandler.GetContracts(clientId, cancellationToken);
            return contracts;
        }

        [HttpPost()]
        public async Task<long> AddClient(ClientRequest model , CancellationToken cancellationToken)
        {
            long clientId = await _clientHandler.AddClient(model, cancellationToken);
            return clientId;
        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<ClientResponse>> GetClient(long clientId, CancellationToken cancellationToken)
        {
            var client = await _clientHandler.GetClientById(clientId, cancellationToken);
            return client;
        }

        [HttpGet()]
        public async Task<List<ClientIdResponse>> GetFilteredClients(string? firstName, string? lastName, string? middlename, DateTime? birthDate, decimal? salary, CancellationToken cancellationToken)
        {
            List<ClientIdResponse> clients = await _clientHandler.GetFilteredClient(firstName, lastName, middlename, birthDate, salary, cancellationToken);
            return clients;
        }
    }
}
