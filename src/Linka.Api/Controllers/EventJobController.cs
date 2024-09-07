using Linka.Application.Common;
using Linka.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class EventJobController(IRepository<EventJob> eventJobRepository) : ControllerBase
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
        public async Task<IEnumerable<EventJob>> GetByEventId(Guid eventId)
        {
            var eventJobs = await eventJobRepository.GetAll(CancellationToken.None);

            return eventJobs.Where(e => e.Event.Id == eventId).ToList();   
        }
    }

    public sealed record GetEventJobByEventId(Guid EventId);
}
