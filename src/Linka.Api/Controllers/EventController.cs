using Linka.Application.Common;
using Linka.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController(IRepository<Event> eventRepository, IRepository<EventJob> eventJobRepository, IRepository<Address> addressRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{eventId}")]
        public async Task<Event> GetById
            (
            Guid eventId
            )
        {
            return await eventRepository.GetFirstAsync(a => a.Id == eventId, CancellationToken.None);
        }

        [HttpGet]
        public async Task<IEnumerable<Event>> GetAll ()
        {
            return await eventRepository.GetAsync(CancellationToken.None);
        }

        [HttpPost]
        public async Task<Event> Create
            (
            CreateEventRequest request
            )
        {
            Address address;
            if (request.Address.Id is null)
            {
                address = new Address
                {
                    Id = Guid.NewGuid(),
                    Cep = request.Address.Cep,
                    City = request.Address.City,
                    Neighborhood = request.Address.Neighborhood,
                    State = request.Address.State,
                    Nickname = request.Address.Nickname,
                    Street = request.Address.Street,
                    Number = request.Address.Number ?? 0,
                };

                await addressRepository.AddAsync(address, CancellationToken.None);

            } else
            {
                address = await addressRepository.GetFirstAsync(a => a.Id == request.Address.Id, CancellationToken.None);
            }

            var @event = new Event
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                Address = address,
            };

            await eventRepository.AddAsync(@event, CancellationToken.None);

            foreach (var job in request.EventJobs) {
                EventJob eventJob = new()
                {
                    Id = Guid.NewGuid(),
                    Title = job.Title,
                    Description = job.Description,
                    MaxVolunteers = job.MaxVolunteers,
                    Event = @event
                };
                await eventJobRepository.AddAsync(eventJob, CancellationToken.None);
            }

            await eventRepository.SaveChangesAsync(CancellationToken.None);

            return @event;
        }
    }

    public sealed record CreateEventRequest(string Title, string Description, DateTime Date, DateTime StartDateTime, DateTime EndDateTime, CreateEventAddress Address, List<CreateEventJob> EventJobs);

    public sealed record CreateEventAddress(Guid? Id, string? Nickname, string? Cep, string? Street, string? Neighborhood, string? State, string? City, int? Number);

    public sealed record CreateEventJob(string Title, string Description, int MaxVolunteers);
}
