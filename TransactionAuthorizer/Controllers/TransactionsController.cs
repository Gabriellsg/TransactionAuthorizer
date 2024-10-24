using Microsoft.AspNetCore.Mvc;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;

namespace TransactionAuthorizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController : ControllerBase
{
    private readonly IAuthorizerAppService _authorizerAppService;
    private readonly ILogger<TransactionsController> _logger;
    private readonly Dictionary<Type, int> _errorCodes = new()
    {
        { typeof(InsufficientBalanceException), 400 },
        { typeof(Exception), 500 }
    };

    public TransactionsController(IAuthorizerAppService authorizerAppService, ILogger<TransactionsController> logger)
    {
        _authorizerAppService = authorizerAppService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Authorize(TransactionRequestModel transaction)
    {
        try
        {
            var authorizationCode = _authorizerAppService.Authorize(transaction);
            _logger.LogInformation("Transaction authorized.");
            return Ok(new { code = authorizationCode });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authorizing transaction.");
            var statusCode = _errorCodes.FirstOrDefault(x => x.Key.IsAssignableFrom(ex.GetType())).Value;
            return StatusCode(statusCode, ex.Message);
        }
    }
}