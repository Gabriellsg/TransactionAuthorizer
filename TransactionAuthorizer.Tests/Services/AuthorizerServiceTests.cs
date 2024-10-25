using Microsoft.Extensions.Logging;
using Moq;
using System.Transactions;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Enums;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;
using TransactionAuthorizer.Domain.Services;

namespace TransactionAuthorizer.Tests.Services;

public class AuthorizerServiceTests
{
    //    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    //    private readonly Mock<ILogger<AuthorizerService>> _loggerMock;
    //    private readonly AuthorizerService _authorizerService;
    //    private readonly SemaphoreSlim _semaphore;

    //    public AuthorizerServiceTests()
    //    {
    //        _transactionRepositoryMock = new Mock<ITransactionRepository>();
    //        _loggerMock = new Mock<ILogger<AuthorizerService>>();
    //        _semaphore = new SemaphoreSlim(1, 1);
    //        _authorizerService = new AuthorizerService(_transactionRepositoryMock.Object, _loggerMock.Object, _semaphore);
    //    }

    //    [Fact]
    //    public async Task AuthorizeTransaction_ValidFlow_ReturnsAuthorized()
    //    {
    //        // Arrange
    //        var transaction = new TransactionDomain { Amount = 100, AccountNumber = "12345" };

    //        // Act
    //        var result = await _service.AuthorizeTransaction(transaction);

    //        // Assert
    //        Assert.True(result.Authorized);
    //    }

    //    [Fact]
    //public async Task AuthorizeTransaction_ValidTransaction_ReturnsAuthorized()
    //{
    //    // Arrange
    //    var transaction = new Transaction { Amount = 100, AccountNumber = "12345" };

    //    // Act
    //    var result = await _service.AuthorizeTransaction(transaction);

    //    // Assert
    //    Assert.True(result.Authorized);
    //}

    //[Fact]
    //public async Task AuthorizeTransaction_InsufficientBalance_ReturnsNotAuthorized()
    //{
    //    // Arrange
    //    var transaction = new Transaction { Amount = 1000, AccountNumber = "12345" };

    //    // Act
    //    var result = await _service.AuthorizeTransaction(transaction);

    //    // Assert
    //    Assert.False(result.Authorized);
    //}

    //[Fact]
    //public async Task AuthorizeTransaction_InvalidAccountNumber_ThrowsException()
    //{
    //    // Arrange
    //    var transaction = new Transaction { Amount = 100, AccountNumber = "" };

    //    // Act e Assert
    //    await Assert.ThrowsAsync<ArgumentException>(() => _service.AuthorizeTransaction(transaction));
    //}
}