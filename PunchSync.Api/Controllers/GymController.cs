using MediatR;
using Microsoft.AspNetCore.Mvc;
using PunchSync.Application.UseCases.Gyms.CreateGym;

namespace PunchSync.Api.Controllers;

public class GymController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGymCommand command, CancellationToken ct) => HandleResult(await mediator.Send(command, ct));
}
