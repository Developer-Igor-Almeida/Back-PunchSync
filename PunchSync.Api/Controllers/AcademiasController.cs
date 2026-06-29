using MediatR;
using Microsoft.AspNetCore.Mvc;
using PunchSync.Application.UseCases.Academias.CreateAcademia;

namespace PunchSync.Api.Controllers;

public class AcademiasController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAcademiaCommand command, CancellationToken ct)
        => HandleResult(await mediator.Send(command, ct));
}
