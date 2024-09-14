using Linka.Domain.Common;
using Linka.Domain.Enums;

namespace Linka.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Address Address { get; set; }
        public byte[]? ImageBytes { get; set; }
        public EventStatus Status { get; set; }
        public Organization Organization { get; set; }

        public static Event Create
            (
            Organization organization,
            string title,
            string description,
            DateTime startDateTime,
            DateTime endDateTime,
            Address address,
            byte[]? imageBytes
            )
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Organization = organization,
                Title = title,
                Description = description,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                Address = address,
                Status = EventStatus.Open, 
                ImageBytes = imageBytes
            };
        }
    }
}
