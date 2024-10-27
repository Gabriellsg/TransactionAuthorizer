using Moq;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Application.Services;
using TransactionAuthorizer.Domain.Entities;
using TransactionAuthorizer.Domain.Exceptions;
using TransactionAuthorizer.Domain.Interfaces;
using FluentValidation.Results;

namespace TransactionAuthorizer.Tests.ApplicationTests;

public class AuthorizerAppServiceTests
{
    private readonly Mock<IAuthorizerService> _mockAuthorizerService;
    private readonly AuthorizerAppService _authorizerAppService;
    private readonly Mock<ITransactionRequestModelValidator> _mockValidatorRequest;

    public AuthorizerAppServiceTests()
    {
        _mockAuthorizerService = new Mock<IAuthorizerService>();
        _mockValidatorRequest = new Mock<ITransactionRequestModelValidator>();
        _authorizerAppService = new AuthorizerAppService(_mockAuthorizerService.Object, _mockValidatorRequest.Object);
    }

    [Fact]
    public async Task AuthorizeTransactionAsync_ShouldReturnApprovedResponse_WhenTransactionIsAuthorized()
    {
        // Arrange
        var transactionRequest = new TransactionRequestModel("123", 50, "5412", "Test Merchant");
        var authorizationResponse = new AuthorizationResponseDomain("00");

        _mockValidatorRequest.Setup(v => v.ValidateAsync(transactionRequest))
            .ReturnsAsync(new ValidationResult());

        _mockAuthorizerService
            .Setup(service => service.AuthorizeAsync(It.IsAny<TransactionDomain>()))
            .ReturnsAsync(authorizationResponse);

        // Act
        var result = await _authorizerAppService.AuthorizeTransactionAsync(transactionRequest);

        // Assert
        Assert.Equal("00", result.Code);
    }

    [Fact]
    public async Task AuthorizeTransactionAsync_ShouldThrowInvalidTransactionException_WhenInsufficientBalance()
    {
        // Arrange
        var transactionRequest = new TransactionRequestModel("123", 150, "5412", "Test Merchant");
        _mockValidatorRequest.Setup(v => v.ValidateAsync(transactionRequest))
            .ReturnsAsync(new ValidationResult());

        _mockAuthorizerService.Setup(service => service.AuthorizeAsync(It.IsAny<TransactionDomain>()))
            .ThrowsAsync(new InsufficientBalanceException("Insufficient balance"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidTransactionException>(() => _authorizerAppService.AuthorizeTransactionAsync(transactionRequest));
        Assert.Equal("Error during transaction authorization.", exception.Message);
        Assert.IsType<InsufficientBalanceException>(exception.InnerException);
    }

    [Fact]
    public async Task AuthorizeTransactionAsync_ShouldThrowInvalidTransactionException_OnGeneralError()
    {
        // Arrange
        var transactionRequest = new TransactionRequestModel("123", 50, "5412", "Test Merchant");
        _mockValidatorRequest.Setup(v => v.ValidateAsync(transactionRequest))
            .ReturnsAsync(new ValidationResult());

        _mockAuthorizerService.Setup(service => service.AuthorizeAsync(It.IsAny<TransactionDomain>()))
            .ThrowsAsync(new Exception("General error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidTransactionException>(() => _authorizerAppService.AuthorizeTransactionAsync(transactionRequest));
        Assert.Equal("Error during transaction authorization.", exception.Message);
        Assert.IsType<Exception>(exception.InnerException);
    }

    [Fact]
    public async Task AuthorizeTransactionAsync_ShouldThrowInvalidTransactionException_WhenValidationFails()
    {
        // Arrange
        var transactionRequest = new TransactionRequestModel("", 50, "5412", "Test Merchant");
        var validationErrors = new List<ValidationFailure>
        {
            new("AccountNumber", "Account number is required.")
        };

        _mockValidatorRequest.Setup(v => v.ValidateAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidTransactionException>(() => _authorizerAppService.AuthorizeTransactionAsync(transactionRequest));
        Assert.Equal("Invalid transaction data: Account number is required.", exception.Message);
    }
}