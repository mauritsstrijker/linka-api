﻿using FluentValidation;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Volunteers.Queries
{
    public sealed record GetVolunteerByIdRequest(Guid VolunteerId) : IRequest<GetVolunteerByIdResponse>;
    
    public sealed record GetVolunteerByIdResponse(string? ProfilePictureBase64, string? ProfilePictureExtension);

    public sealed class GetVolunteerByIdHandler
        (
        IVolunteerRepository volunteerRepository
        ) : IRequestHandler<GetVolunteerByIdRequest, GetVolunteerByIdResponse>
    {
        public async Task<GetVolunteerByIdResponse> Handle(GetVolunteerByIdRequest request, CancellationToken cancellationToken)
        {
            var volunteer = await volunteerRepository.Get(request.VolunteerId, cancellationToken) ?? throw new Exception();

            return new GetVolunteerByIdResponse(
                volunteer.ProfilePictureBytes != null ? Convert.ToBase64String(volunteer.ProfilePictureBytes) : null,
                volunteer.ProfilePictureExtension
            );

        }
    }

    public sealed class GetVolunteerByIdValidator : AbstractValidator<GetVolunteerByIdRequest>
    {
        public GetVolunteerByIdValidator()
        {
            RuleFor(x => x.VolunteerId).NotEmpty();
        }
    }
}