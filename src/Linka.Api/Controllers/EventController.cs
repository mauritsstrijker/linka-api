using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Features.Events.Commands;
using Linka.Application.Features.Events.Queries;
using Linka.Application.Mappers;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController(IEventRepository eventRepository, IRepository<EventJob> eventJobRepository, IRepository<Address> addressRepository, IUnitOfWork unitOfWork, IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("{eventId}")]
        public async Task<EventDTO> GetById
            (
            Guid eventId
            )
        {
            var @event = await eventRepository.Get(eventId, CancellationToken.None);
            return EventMapper.MapToEventDto(@event);
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<EventDTO>> GetAll ()
        {
            var events = await eventRepository.GetAll(CancellationToken.None);
            return events.Select(e => EventMapper.MapToEventDto(e));
        }

        [Authorize(Policy = "Organization")]
        [HttpPost]
        public async Task<CreateEventResponse> Create
            (
            [FromBody] CreateEventRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize(Policy = "Organization")]
        [HttpPut]
        [Route("{id}")]
        public async Task<UpdateEventResponse> Update
           (
           [FromBody] UpdateEventRequest request,
           [FromRoute] Guid id,
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(request with { Id = id }, cancellationToken);
        }

        [Authorize]
        [HttpGet]
        [Route("organization/{organizationId}")]
        public async Task<List<GetAllEventByOrganizationIdResponse>> GetAllByOrganizationId
           (
           [FromRoute] Guid organizationId,
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(new GetAllEventByOrganizationIdRequest(organizationId), cancellationToken);
        }

        [Authorize(Policy = "Organization")]
        [HttpPost]
        [Route("{eventId}/cancel")]
        public async Task<CancelEventResponse> Cancel
            (
            [FromRoute] Guid eventId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new CancelEventRequest(eventId), cancellationToken);
        }
    }

    public sealed record CreateEventAddress(Guid? Id, string? Nickname, string? Cep, string? Street, string? Neighborhood, string? State, string? City, int? Number);

    public sealed record CreateEventJob(string Title, string Description, int MaxVolunteers);
}
