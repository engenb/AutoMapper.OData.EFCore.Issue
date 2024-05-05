using Domain.Components.Foos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OData.Controllers;

[ApiController]
[ApiVersion("1")]
[ControllerName($"{nameof(ApiModel.Foo)}s")]
[Route("api/v{version:apiVersion}")]
[Consumes("application/json")]
[Produces("application/json")]
public class FoosV1Controller : ODataController
{
    private readonly IMediator _mediator;

    public FoosV1Controller(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("Foos")]
    [ProducesResponseType(typeof(ODataCollectionResponseType<ApiModel.Foo>), Status200OK)]
    public async Task<IActionResult> Foos(ODataQueryOptions<ApiModel.Foo> queryOptions, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFoosODataQuery(queryOptions), ct);

        return Ok(result);
    }
}