using Linka.Domain.Common;

namespace Linka.Domain.Entities;

public class Address : BaseEntity
{
    public string Cep { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string Neighborhood { get; set; }
    public string State { get; set; }
    public string? Nickname { get; set; }

    public static Address Create
        (
        string cep,
        string city,
        string street,
        int number,
        string neighborhood,
        string state,
        string? nickname
        )
    {
        return new Address
        {
            Id = Guid.NewGuid(),
            Cep = cep.Trim(),
            City = city.Trim(),
            Street = street.Trim(),
            Number = number,
            Neighborhood = neighborhood.Trim(),
            State = state.Trim(),
            Nickname = nickname ?? null
        };
    }
}