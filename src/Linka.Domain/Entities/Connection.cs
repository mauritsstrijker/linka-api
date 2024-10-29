using Linka.Domain.Common;

namespace Linka.Domain.Entities;
public class Connection : BaseEntity
{
    public Volunteer Volunteer1 { get; set; }
    public Volunteer Volunteer2 { get; set; }
    public static Connection Create(Volunteer volunteer1, Volunteer volunteer2)
    {
        return new Connection
        {
            Id = Guid.NewGuid(),
            Volunteer1 = volunteer1,
            Volunteer2 = volunteer2,
        };
    }
}
