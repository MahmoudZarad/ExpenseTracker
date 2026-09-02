using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator _mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCurrentUserQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(
        [FromBody] UpdateUserSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new UpdateUserSettingsCommand(request),
            cancellationToken);

        return Ok(result);
    }
}