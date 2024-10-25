using Microsoft.AspNetCore.Mvc;
using TransactionAuthorizer.Application.Interfaces;
using TransactionAuthorizer.Application.Models;
using TransactionAuthorizer.Domain.Exceptions;

namespace TransactionAuthorizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController(IAuthorizerAppService authorizerAppService, ILogger<TransactionsController> logger) : ControllerBase
{
    private readonly IAuthorizerAppService _authorizerAppService = authorizerAppService;
    private readonly ILogger<TransactionsController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] TransactionRequestModel transaction)
    {
        try
        {
            var authorizationCode = await _authorizerAppService.AuthorizeTransactionAsync(transaction);
            _logger.LogInformation("Transaction authorized.");
            return Ok(new { code = authorizationCode });
        }    
        catch (InvalidTransactionException ex)
        {
            _logger.LogWarning(ex, "Invalid transaction.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authorizing transaction.");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}
