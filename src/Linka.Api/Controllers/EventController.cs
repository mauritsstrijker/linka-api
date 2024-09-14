using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Features.Events.Commands;
using Linka.Application.Mappers;
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
    public class EventController(IRepository<Event> eventRepository, IRepository<EventJob> eventJobRepository, IRepository<Address> addressRepository, IUnitOfWork unitOfWork, IMediator mediator) : ControllerBase
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

        [Authorize]
        [HttpPost]
        public async Task<CreateEventResponse> Create
            (
            [FromBody] CreateEventRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }
    }

    public sealed record CreateEventAddress(Guid? Id, string? Nickname, string? Cep, string? Street, string? Neighborhood, string? State, string? City, int? Number);

    public sealed record CreateEventJob(string Title, string Description, int MaxVolunteers);
}
