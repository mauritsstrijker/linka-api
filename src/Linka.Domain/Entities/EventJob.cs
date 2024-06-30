using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class EventJob : BaseEntity
    {
        public Event Event { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxVolunteers { get; set; }
    }
}
