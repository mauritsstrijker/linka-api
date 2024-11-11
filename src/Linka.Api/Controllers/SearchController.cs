using Linka.Application.Features.Search;
using Linka.Application.Features.Users.Commands;
using Linka.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("search")]
        public async Task<SearchResponse> Search
            (
            CancellationToken cancellationToken,
            [FromQuery] string searchTerm
            )
        {
            return await mediator.Send(new SearchRequest { Search = searchTerm  }, cancellationToken);
        }
    }
}
