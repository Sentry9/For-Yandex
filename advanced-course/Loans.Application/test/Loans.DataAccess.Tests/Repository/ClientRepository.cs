using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.Models;
using Loans.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Loans.DataAccess.Test.Repository
{
    public class ClientRepositoryTests
    {
        [Fact]
        public async Task GetClientByIdAsync_ValidId_ReturnsClient()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetClientByIdAsync_ValidId_ReturnsClient")
                .Options;

            using var context = new DataContext(options);
            var repository = new ClientRepository(context);

            long clientId = 1;
            var client = new ClientEntity
            {
                Id = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                BirthDate = DateTime.Today,
                Salary = 10000
            };

            context.Clients.Add(client);
            context.SaveChanges();

            // Act
            var result = await repository.GetClientByIdAsync(clientId, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
            Assert.Equal(client.FirstName, result.FirstName);
            Assert.Equal(client.LastName, result.LastName);
            Assert.Equal(client.MiddleName, result.MiddleName);
            Assert.Equal(client.Salary, result.Salary);
            Assert.Equal(client.BirthDate, result.BirthDate);
        }

        [Fact]
        public async Task GetClientByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetClientByIdAsync_InvalidId_ReturnsNull")
                .Options;

            using var context = new DataContext(options);
            var repository = new ClientRepository(context);

            long invalidClientId = 999;

            // Act
            var result = await repository.GetClientByIdAsync(invalidClientId, new CancellationToken());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetClientsAsync_WithFilter_ReturnsFilteredClients()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetClientsAsync_WithFilter_ReturnsFilteredClients")
                .Options;

            using var context = new DataContext(options);
            var repository = new ClientRepository(context);

            var clients = new List<ClientEntity>
            {
                new ClientEntity {FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", BirthDate = DateTime.Today, Salary = 10000},
                new ClientEntity {FirstName = "Антон", LastName = "Иванов", MiddleName = "Иванович", BirthDate = DateTime.Today, Salary = 10000 },
                new ClientEntity {FirstName = "Дмитрий", LastName = "Иванов", MiddleName = "Иванович", BirthDate = DateTime.Today, Salary = 10000 },
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();

            var requiredClient = clients[2];

            var model = new ClientFilterRequest()
            {
                Firstname = clients[2].FirstName,
            };

            // Act
            var filteredClients = await repository.GetClientsAsync(model, new CancellationToken());

            // Assert
            Assert.NotNull(filteredClients);
            Assert.Single(filteredClients);
            Assert.Equal(requiredClient.Id, filteredClients[0].Id);
        }

        [Fact]
        public async Task CreateClientAsync_NewClient_ReturnsClientId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "CreateClientAsync_NewClient_ReturnsClientId")
                .Options;

            using var context = new DataContext(options);
            var repository = new ClientRepository(context);

            var newClient = new ClientModel { Id = 1, FirstName = "Новый клиент", LastName = "Новый клиент", MiddleName = "Новый клиент", Salary = 20000, BirthDate = new DateTime(2000, 2, 3)};

            // Act
            long clientId = await repository.CreateClientAsync(newClient, new CancellationToken());

            // Assert
            Assert.True(clientId > 0);
            Assert.NotNull(await context.Clients.FindAsync(clientId));
        }

        [Fact]
        public async Task UpdateClientAsync_ExistingClient_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateClientAsync_ExistingClient_Success")
                .Options;

            using var context = new DataContext(options);
            var repository = new ClientRepository(context);

            var client = new ClientEntity { Id = 1, FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", BirthDate = new DateTime(2000, 3, 2), Salary = 20000};
            context.Clients.Add(client);
            context.SaveChanges();

            var updatedClient = new ClientModel { Id = 1, FirstName = "Обновлённое имя", LastName = "Иванов", MiddleName = "Иванович", BirthDate = new DateTime(2000, 3, 2), Salary = 20000 };

            // Act
            await repository.UpdateClientAsync(updatedClient, new CancellationToken());

            // Assert
            var result = await context.Clients.FindAsync((long)1);
            Assert.Equal(updatedClient.FirstName, result.FirstName);
        }
    }
}
