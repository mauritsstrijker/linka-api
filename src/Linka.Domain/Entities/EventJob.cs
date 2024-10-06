using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class EventJob : BaseEntity
    {
        public Event Event { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxVolunteers { get; set; }
        public List<Volunteer> Volunteers { get; set; }

        public static EventJob Create
            (
            Event @event,
            string title,
            string description,
            int maxVolunteers
            )
        {
            return new EventJob
            {
                Id = Guid.NewGuid(),
                Event = @event,
                Title = title,
                Description = description,
                MaxVolunteers = maxVolunteers
            };
        }
    }
}
