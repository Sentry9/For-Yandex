using Loans.Api.Contracts.Status;
using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.Exceptions;
using Loans.DataAccess.Models;
using Loans.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Loans.DataAccess.Test.Repository
{
    public class LoanRepositoryTests
    {
        [Fact]
        public async Task GetLoanByIdAsync_ValidId_ReturnsLoan()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetLoanByIdAsync_ValidId_ReturnsLoan")
                .Options;

            using var context = new DataContext(options);
            var repository = new LoanRepository(context);
            
            var client = new ClientEntity
            {
                Id = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                Salary = 20000,
                BirthDate = new DateTime(2000, 3, 2)
            };


            long loanId = 1;
            var loan = new LoanEntity()
            {
                Id = loanId,
                ClientId = 1,
                Client = client,
                TermInYears = 5,
                Amount = 10000,
                ExpectedInterestRate = 5.0m,
                CreationDate = DateTime.Now,
                Status = LoansStatus.Denied,
                RejectReason = ""
            };

            context.Loans.Add(loan);
            context.SaveChanges();

            // Act
            var result = await repository.GetLoanByIdAsync(loanId, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loanId, result.Id);
            Assert.Equal(loan.ClientId, result.ClientModel.Id);
            Assert.Equal(loan.TermInYears, result.TermInYears);
            Assert.Equal(loan.Amount, result.Amount);
            Assert.Equal(loan.ExpectedInterestRate, result.ExpectedInterestRate);
            Assert.Equal(loan.CreationDate, result.CreationDate);
        }

        [Fact]
        public async Task GetLoanByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetLoanByIdAsync_InvalidId_ReturnsNull")
                .Options;

            using var context = new DataContext(options);
            var repository = new LoanRepository(context);

            long invalidLoanId = 999;

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                var result = await repository.GetLoanByIdAsync(invalidLoanId, new CancellationToken());
            });
        }

        [Fact]
        public async Task GetLoansByClientIdAsync_ValidClientId_ReturnsLoans()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetLoansByClientIdAsync_ValidClientId_ReturnsLoans")
                .Options;

            using var context = new DataContext(options);
            var repository = new LoanRepository(context);
            var client = new ClientEntity()
            {
                Id = 1,
                FirstName = "Иван",
                LastName = "",
                MiddleName = "",
                Salary = 20000,
                BirthDate = new DateTime(2000, 3, 2)
            };
            var client2 = new ClientEntity()
            {
                Id = 2,
                FirstName = "Иван",
                LastName = "",
                MiddleName = "",
                Salary = 20000,
                BirthDate = new DateTime(2000, 3, 2)
            };

            long clientId = client.Id;
            var loans = new List<LoanEntity>
            {
                new LoanEntity { Id = 1, ClientId = clientId, TermInYears = 5, Amount = 10000, ExpectedInterestRate = 5.0m, CreationDate = DateTime.Now, Status = LoansStatus.Denied, RejectReason = "", Client = client},
                new LoanEntity { Id = 2, ClientId = clientId, TermInYears = 3, Amount = 7500, ExpectedInterestRate = 4.5m, CreationDate = DateTime.Now, Status = LoansStatus.Denied, RejectReason = "", Client = client},
                new LoanEntity { Id = 3, ClientId = 2, TermInYears = 7, Amount = 15000, ExpectedInterestRate = 6.0m, CreationDate = DateTime.Now, Status = LoansStatus.Denied, RejectReason = "", Client = client2},
            };

            context.Loans.AddRange(loans);
            context.SaveChanges();

            // Act
            var result = (await repository.GetLoansByClientIdAsync(clientId, new CancellationToken())).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, loan => Assert.Equal(clientId, loan.ClientModel.Id));
        }

        [Fact]
        public async Task CreateLoanAsync_NewLoan_ReturnsLoanId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "CreateLoanAsync_NewLoan_ReturnsLoanId")
                .Options;

            using var context = new DataContext(options);
            var repository = new LoanRepository(context);
            var client = new ClientModel
            {
                FirstName = "Иван",
                LastName = "",
                MiddleName = "",
                Salary = 20000,
                BirthDate = new DateTime(2000, 3, 2)
            };

            var newLoan = new LoanApplicationModel
            {
                ClientModel = client,
                TermInYears = 4,
                Amount = 8000,
                ExpectedInterestRate = 4.0m,
                CreationDate = DateTime.Now
            };

            // Act
            long loanId = await repository.CreateLoanAsync(newLoan, new CancellationToken());

            // Assert
            Assert.True(loanId > 0);
            Assert.NotNull(await context.Loans.FindAsync(loanId));
        }

        [Fact]
        public async Task UpdateLoanAsync_ExistingLoan_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateLoanAsync_ExistingLoan_Success")
                .Options;

            using var context = new DataContext(options);
            var repository = new LoanRepository(context);
            var client = new ClientModel
            {
                FirstName = "Иван",
                LastName = "",
                MiddleName = "",
                Salary = 20000,
                BirthDate = new DateTime(2000, 3, 2)
            };

            var loan = new LoanEntity
            {
                Id = 1,
                ClientId = 1,
                TermInYears = 5,
                Amount = 10000,
                ExpectedInterestRate = 5.0m,
                CreationDate = DateTime.Now,
                Status = LoansStatus.Denied,
                RejectReason = ""
            };

            context.Loans.Add(loan);
            context.SaveChanges();

            var updatedLoan = new LoanApplicationModel
            {
                Id = 1,
                ClientModel = client,
                TermInYears = 5,
                Amount = 12000,
                ExpectedInterestRate = 5.0m,
                CreationDate = DateTime.Now
            };

            // Act
            await repository.UpdateLoanAsync(updatedLoan, new CancellationToken());

            // Assert
            var result = await context.Loans.FindAsync((long)1);
            Assert.Equal(updatedLoan.Amount, result.Amount);
        }
    }
}
