namespace Linka.Application.Dtos
{
    public sealed record CreateAddressDto
        (
            string Cep,
            string City,
            string Street,
            int Number,
            string Neighborhood,
            string State,
            string? Nickname
        );
}
