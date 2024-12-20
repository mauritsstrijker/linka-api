﻿using Linka.Domain.Entities;
using Linka.Domain.Enums;

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
            return new EventDTO(@event.Id, @event.Organization.Id, @event.Title, @event.Description, @event.StartDateTime, @event.EndDateTime, @event.Address, @event.Status, imageBase64);
        }
    }
    public sealed record EventDTO(Guid Id, Guid OrganizationId, string Title, string Description, DateTime StartDateTime, DateTime EndDateTime, Address Address, EventStatus Status, string ImageBase64);

}

