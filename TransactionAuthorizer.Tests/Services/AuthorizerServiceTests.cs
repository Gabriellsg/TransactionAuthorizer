//using Microsoft.Extensions.Logging;
//using Moq;
//using TransactionAuthorizer.Domain.Entities;
//using TransactionAuthorizer.Domain.Enums;
//using TransactionAuthorizer.Domain.Exceptions;
//using TransactionAuthorizer.Domain.Interfaces;
//using TransactionAuthorizer.Domain.Services;

//namespace TransactionAuthorizer.Tests.Services;

//public class AuthorizerServiceTests
//{
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
//    public async Task AuthorizeAsync_ValidTransaction_ReturnsApproved()
//    {
//        // Arrange
//        var transaction = new TransactionDomain("123", 1, "5811", "PADARIA DO ZE");

//        _transactionRepositoryMock
//            .Setup(repo => repo.GetBenefitBalanceAsync(transaction.AccountNumber, "Meal"))
//            .ReturnsAsync(100);

//        // Act
//        var result = await _authorizerService.AuthorizeAsync(transaction);

//        // Assert
//        Assert.Equal(AuthorizationCode.Approved, result);
//        _transactionRepositoryMock
//            .Verify(repo => repo.UpdateAccountBalanceAsync(transaction.AccountNumber, "Meal", transaction.TotalAmount), Times.Once);
//    }

//    [Fact]
//    public async Task AuthorizeAsync_InsufficientBalance_ThrowsInsufficientBalanceException()
//    {
//        // Arrange
//        var transaction = new TransactionDomain("123", 1, "5412", "PADARIA DO ZE");
    

//        _transactionRepositoryMock
//            .Setup(repo => repo.GetBenefitBalanceAsync(transaction.AccountNumber, "Meal"))
//            .ReturnsAsync(50);

//        // Act & Assert
//        var exception = await Assert.ThrowsAsync<InsufficientBalanceException>(() => _authorizerService.AuthorizeAsync(transaction));
//        Assert.Equal("The balance for the benefit category is insufficient to authorize the transaction.", exception.Message);
//    }
//}
