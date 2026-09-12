using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DashboardDto>> GetDashboard(
        [FromQuery] GetDashboardQuery query)
    {
        var result = await sender.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Value);
    }
}
