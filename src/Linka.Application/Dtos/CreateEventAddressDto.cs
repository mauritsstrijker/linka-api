namespace Linka.Application.Dtos
{
    public sealed record CreateEventAddressDto(Guid? Id, string? Nickname, string? Cep, string? Street, string? Neighborhood, string? State, string? City, int? Number);
}
