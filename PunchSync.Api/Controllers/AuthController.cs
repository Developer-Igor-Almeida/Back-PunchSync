using MediatR;
using Microsoft.AspNetCore.Mvc;
using PunchSync.Application.UseCases.Auth.Login;
using PunchSync.Application.UseCases.Auth.Logout;
using PunchSync.Application.UseCases.Auth.RefreshToken;
using PunchSync.Application.UseCases.Auth.Register;

namespace PunchSync.Api.Controllers;

public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
        => HandleResult(await mediator.Send(command, ct));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        => HandleResult(await mediator.Send(command, ct));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
        => HandleResult(await mediator.Send(command, ct));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken ct)
        => HandleResult(await mediator.Send(command, ct));
}
