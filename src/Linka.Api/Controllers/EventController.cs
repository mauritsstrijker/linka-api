using Linka.Application.Common;
using Linka.Application.Mappers;
using Linka.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController(IRepository<Event> eventRepository, IRepository<EventJob> eventJobRepository, IRepository<Address> addressRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{eventId}")]
        public async Task<EventModel> GetById
            (
            Guid eventId
            )
        {
            var @event = await eventRepository.GetFirstAsync(a => a.Id == eventId, CancellationToken.None);
            return EventMapper.MapToEventDto(@event);
        }

        [HttpGet]
        public async Task<IEnumerable<EventModel>> GetAll ()
        {
            var events = await eventRepository.GetAsync(CancellationToken.None);
            return events.Select(e => EventMapper.MapToEventDto(e));
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
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                Address = address,
            };

            if (!request.ImageBase64.IsNullOrEmpty())
            {
                var imageBytes = Convert.FromBase64String(request.ImageBase64);
                @event.ImageBytes = imageBytes;
            }

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

    public sealed record CreateEventRequest(string Title, string Description, DateTime Date, DateTime StartDateTime, DateTime EndDateTime, CreateEventAddress Address, List<CreateEventJob> EventJobs, string ImageBase64);

    public sealed record CreateEventAddress(Guid? Id, string? Nickname, string? Cep, string? Street, string? Neighborhood, string? State, string? City, int? Number);

    public sealed record CreateEventJob(string Title, string Description, int MaxVolunteers);
}
