using ExpenseTracker.Application.Common.Auth;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {

        return Ok(await _mediator.Send(
            new RegisterCommand(request),
            cancellationToken));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(
            new LoginCommand(request),
            cancellationToken));
    }
}
