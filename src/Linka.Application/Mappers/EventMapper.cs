using Linka.Domain.Entities;

namespace Linka.Application.Mappers
{
    public static class EventMapper
    {
        public static EventDTO MapToEventDto(Event @event)
        {
            string imageBase64 = null;
            if (@event.ImageBytes != null)
            {
                imageBase64 = Convert.ToBase64String(@event.ImageBytes);
            }
            return new EventDTO(@event.Id, @event.Title, @event.Description, @event.StartDateTime, @event.EndDateTime, @event.Address, imageBase64);
        }
    }
    public sealed record EventDTO(Guid Id, string Title, string Description, DateTime StartDateTime, DateTime EndDateTime, Address Address, string ImageBase64);

}

