using Linka.Domain.Entities;
using System.Collections;
using System.Diagnostics.Tracing;

namespace Linka.Application.Mappers
{
    public static class EventMapper
    {
        public static EventModel MapToEventDto(Event @event)
        {
            string imageBase64 = null;
            if (@event.ImageBytes != null)
            {
                imageBase64 = Convert.ToBase64String(@event.ImageBytes);
            }
            return new EventModel(@event.Title, @event.Description, @event.StartDateTime, @event.EndDateTime, @event.Address, imageBase64);
        }
    }
    public sealed record EventModel(string Title, string Description, DateTime StartDateTime, DateTime EndDateTime, Address Address, string ImageBase64);

}

