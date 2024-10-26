using Microsoft.Extensions.Logging;
using Moq;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;
using TransactionAuthorizer.Domain.Services;

namespace TransactionAuthorizer.Tests.DomainTests;

public class AuthorizerServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository = new();
    private readonly Mock<IBenefitCategoryRepository> _mockBenefitCategoryRepository = new();
    private readonly Mock<ITransactionLogRepository> _mockTransactionLogRepository = new();
    private readonly Mock<IMerchantRepository> _mockMerchantRepository = new();
    private readonly Mock<ILogger<AuthorizerService>> _mockLogger = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly AuthorizerService _authorizerService;

    public AuthorizerServiceTests()
    {
        _authorizerService = new AuthorizerService(
            _mockLogger.Object,
            _mockAccountRepository.Object,
            _mockBenefitCategoryRepository.Object,
            _mockTransactionLogRepository.Object,
            _mockMerchantRepository.Object,
            _semaphore
        );
    }

    [Fact]
    public async Task AuthorizeTransaction_ShouldReturnCode00_WhenApproved()
    {
        // Arrange
        var transaction = new TransactionDomain("123", 50, "5412", "UBER EATS");
       
        _mockAccountRepository
            .Setup(repo => repo.GetAccountAsync("123"))
            .ReturnsAsync(new AccountDomain(1, "123", 100, 100, 100));

        _mockBenefitCategoryRepository
            .Setup(repo => repo.GetBenefitCategoryAsync("5412"))
            .ReturnsAsync(new BenefitCategoryDomain(1, "Food", 1000));

        _mockAccountRepository
            .Setup(repo => repo.GetBenefitBalanceAsync("123", It.IsAny<BenefitCategoryDomain>()))
            .ReturnsAsync(100);

        // Act
        var response = await _authorizerService.AuthorizeAsync(transaction);

        // Assert
        Assert.Equal("00", response.Code);
    }

    [Fact]
    public async Task AuthorizeTransaction_ShouldReturnCode51_WhenInsufficientBalance()
    {
        // Arrange
        var transaction = new TransactionDomain("123", 200, "5819", "UBER EATS");

        _mockAccountRepository
            .Setup(repo => repo.GetAccountAsync("123"))
            .ReturnsAsync(new AccountDomain(1, "123", 50, 100, 100));

        _mockBenefitCategoryRepository
            .Setup(repo => repo.GetBenefitCategoryAsync("5819"))
            .ReturnsAsync(new BenefitCategoryDomain(1, "Food", 1000));
        
        _mockBenefitCategoryRepository
            .Setup(repo => repo.GetDefaultBenefitCategoryAsync())
            .ReturnsAsync(new BenefitCategoryDomain(1, "Food", 1000));

        _mockAccountRepository
            .Setup(repo => repo.GetBenefitBalanceAsync("123", new BenefitCategoryDomain(1, "Food", 1000)))
            .ReturnsAsync(50);

        // Act
        var result = await _authorizerService.AuthorizeAsync(transaction);

        // Assert
        Assert.Equal("51", result.Code);
    }

    [Fact]
    public async Task AuthorizeTransaction_ShouldReturnCode07_OnUnexpectedError()
    {
        // Arrange
        var transaction = new TransactionDomain("123", 50, "5412", "PADARIA DO ZE");

        _mockAccountRepository
            .Setup(repo => repo.GetAccountAsync("123"))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var response = await _authorizerService.AuthorizeAsync(transaction);

        // Assert
        Assert.Equal("07", response.Code);
    }

    [Fact]
    public async Task AuthorizeTransaction_ShouldThrowInvalidTransactionAmountException_WhenTotalAmountIsZeroOrLess()
    {
        // Arrange
        var invalidTransaction = new TransactionDomain("123", 0, "5412", "PADARIA DO ZE");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidTransactionAmountException>(() => _authorizerService.AuthorizeAsync(invalidTransaction));
        Assert.Equal("Total amount 0 must be greater than zero.", exception.Message);
    }
}