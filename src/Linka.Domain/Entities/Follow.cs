using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Follow : BaseEntity
    {
        public Organization Organization { get; set; }
        public Volunteer Volunteer { get; set; }
        public static Follow Create(Organization organization, Volunteer volunteer)
        {
            return new Follow
            {
                Id = Guid.NewGuid(),
                Organization = organization,
                Volunteer = volunteer
            };
        }
    }
}
