using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class JobVolunteerActivity : BaseEntity
    {
        public EventJob Job { get; set; }
        public Volunteer Volunteer { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set;}
    }
}
