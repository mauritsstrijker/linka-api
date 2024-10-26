using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Follow : BaseEntity
    {
        public Organization Organization { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
