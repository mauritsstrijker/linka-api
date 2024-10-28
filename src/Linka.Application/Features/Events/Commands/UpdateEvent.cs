using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record UpdateEventRequest
      (
            Guid Id,
            string Title,
            string Description,
            string? ImageBase64,
            CreateAddressDto Address
        ) : IRequest<UpdateEventResponse>;

    public sealed record UpdateEventResponse;

    public class UpdateEventHander
        (
        IEventRepository eventRepository,
        IRepository<Address> addressRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<UpdateEventRequest, UpdateEventResponse>
    {
        public async Task<UpdateEventResponse> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            var @event = await eventRepository.Get(request.Id, cancellationToken) ?? throw new Exception("Evento nao foi encontrado.");

            var address = await addressRepository.Get(@event.Address.Id, cancellationToken);

            address.Street = request.Address.Street;
            address.City = request.Address.City;    
            address.Cep = request.Address.Cep;
            address.Neighborhood = request.Address.Neighborhood;
            address.State = request.Address.State;
            address.Number = request.Address.Number;

            await addressRepository.Update(address, cancellationToken);

            var eventImage = request.ImageBase64 is null ? null : Convert.FromBase64String(request.ImageBase64);

            @event.Update(request.Title, request.Description, eventImage);

            await eventRepository.Update(@event, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new UpdateEventResponse();
        }
    }

    public class UpdateEventValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.ImageBase64)
              .MustAsync(async (base64, cancellationToken) => await ProfilePictureHelper.ValidateImageAsync(Convert.FromBase64String(base64), 1080, 450))
              .When(x => x.ImageBase64 != null)
              .WithMessage("Imagem de perfil inválida.");
        }
        private bool NotBeInThePast(DateTime startDateTime)
        {
            return startDateTime >= DateTime.Now;
        }
    }
}
