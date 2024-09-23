using Linka.Application.Common;
using Linka.Application.Features.EventJobs;
using Linka.Application.Features.EventJobs.Commands;
using Linka.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class EventJobController(IRepository<EventJob> eventJobRepository, IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("{eventJobId}")]
        public async Task<EventJob> GetById
            (
            Guid eventJobId
            )
        {
            return await eventJobRepository.Get(eventJobId, CancellationToken.None);
        }
        [HttpGet]
        public async Task<IEnumerable<EventJob>> GetAll()
        {
            return await eventJobRepository.GetAll(CancellationToken.None);
        }
        [HttpGet]
        [Route("event/{eventId}")]
        public async Task<IEnumerable<GetEventJobsByEventIdResponse>> GetEventJobsByEventId
            (
            [FromRoute] Guid eventId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetEventJobsByEventIdRequest(eventId), cancellationToken);
        }

        [HttpPost]
        [Route("{eventJobId}/subscribe")]
        public async Task<SubscribeVolunteerToJobResponse> Subscribe
            (
            [FromRoute] Guid eventJobId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new SubscribeVolunteerToJobRequest(eventJobId), cancellationToken);
        }

        [HttpPost]
        [Route("{eventJobId}/unsubscribe")]
        public async Task<UnsubscribeVolunteerToJobResponse> Unsubscribe
            (
            [FromRoute] Guid eventJobId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new UnsubscribeVolunteerToJobRequest(eventJobId), cancellationToken);
        }
    }

    public sealed record GetEventJobByEventId(Guid EventId);
}
