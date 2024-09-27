﻿using FluentValidation;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Organizations.Queries
{
    public sealed record GetOrganizationByIdRequest
        (
        Guid Id
        ) : IRequest<GetOrganizationByIdResponse>;

    public sealed record GetOrganizationByIdResponse
        (
            string CompanyName,
            string TradingName,
            string Phone,
            AddressDto Address,
            string ProfilePictureBase64
        );

    public class GetOrganizationByIdHandler
        (
        IOrganizationRepository organizationRepository
        ) : IRequestHandler<GetOrganizationByIdRequest, GetOrganizationByIdResponse>
    {
        public async Task<GetOrganizationByIdResponse> Handle(GetOrganizationByIdRequest request, CancellationToken cancellationToken)
        {
            var organization = await organizationRepository.Get(request.Id, cancellationToken) ?? throw new Exception("Organização não encontrada.");

            return MapToDto(organization);
        }

        public static GetOrganizationByIdResponse MapToDto(Organization organization)
        {
            return new GetOrganizationByIdResponse
        (
            CompanyName: organization.CompanyName,
            TradingName: organization.TradingName,
            Phone: organization.Phone,
            Address: new AddressDto
            (
                Cep: organization.Address.Cep,
                City: organization.Address.City,
                Street: organization.Address.Street,
                Number: organization.Address.Number,
                Neighborhood: organization.Address.Neighborhood,
                State: organization.Address.State,
                Nickname: organization.Address.Nickname
            ),
            ProfilePictureBase64: organization.ProfilePictureBytes != null
                ? Convert.ToBase64String(organization.ProfilePictureBytes)
                : null
        );
        }
    }
    public class GetOrganizationByIdValidator : AbstractValidator<GetOrganizationByIdRequest>
    {
        public GetOrganizationByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
