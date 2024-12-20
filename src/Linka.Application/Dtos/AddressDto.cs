﻿namespace Linka.Application.Dtos
{
    public sealed record AddressDto
    (
        Guid Id,
        string Cep,
        string City,
        string Street,
        int Number,
        string Neighborhood,
        string State,
        string? Nickname
    );
}
