using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Helpers;
using Linka.Domain.Entities;
using MediatR;
using System.Net;
using System.Threading;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record CreateEventRequest
        (
            string Title,
            string Description,
            DateTime StartDateTime,
            DateTime EndDateTime,
            CreateEventAddressDto Address,
            List<CreateEventJobDto> EventJobs,
            string? ImageBase64
        ) : IRequest<CreateEventResponse>;

    public sealed record CreateEventResponse;

    public class CreateEventHandler
        (
        IRepository<Address> addressRepository,
        IRepository<Event> eventRepository,
        IRepository<EventJob> eventJobRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<CreateEventRequest, CreateEventResponse>
    {
        public async Task<CreateEventResponse> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var address = await CreateOrGetAddress(request, cancellationToken);

            var @event = await CreateEvent(request, address, cancellationToken);

            await CreateEventJobs(request, @event, cancellationToken);
            
            await unitOfWork.Commit(cancellationToken);

            return new CreateEventResponse();
        }

        public async Task<Address> CreateOrGetAddress(CreateEventRequest request, CancellationToken cancellationToken)
        {
            Address address;

            if (request.Address.Id is Guid addressId)
            {
                address = await addressRepository.Get(addressId, cancellationToken);
            }
            else
            {
                address = Address.Create(request.Address.Cep, request.Address.City, request.Address.Street, request.Address.Number ?? default, request.Address.Neighborhood, request.Address.State, request.Address.Nickname);
              
                await addressRepository.Insert(address, cancellationToken);
            }

            return address;
        }

        public async Task<Event> CreateEvent(CreateEventRequest request, Address address, CancellationToken cancellationToken)
        {
            var eventImage = request.ImageBase64 is null ? null : Convert.FromBase64String(request.ImageBase64);

            var @event = Event.Create(request.Title, request.Description, request.StartDateTime, request.EndDateTime, address, eventImage);

            await eventRepository.Insert(@event, cancellationToken);

            return @event;
        }

        public async Task CreateEventJobs(CreateEventRequest request, Event @event, CancellationToken cancellationToken)
        {
            foreach (var eventJob in request.EventJobs)
            {
                await eventJobRepository.Insert(EventJob.Create(@event, eventJob.Title, eventJob.Description, eventJob.MaxVolunteers), cancellationToken);
            }
        }
    }

    public class CreateEventValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(e => e.StartDateTime)
            .Must(NotBeInThePast)
            .WithMessage("A data de início não pode estar no passado.");

            RuleFor(e => e.EndDateTime)
                .GreaterThan(e => e.StartDateTime)
                .WithMessage("A data de término deve ser posterior à data de início.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Endereço é obrigatório.")
                .ChildRules(address =>
                {
                    address.RuleFor(x => x.Cep)
                        .NotEmpty().WithMessage("CEP é obrigatório.")
                        .Length(8).WithMessage("CEP deve ter 8 caracteres.");

                    address.RuleFor(x => x.City)
                        .NotEmpty().WithMessage("Cidade é obrigatória.");

                    address.RuleFor(x => x.Street)
                        .NotEmpty().WithMessage("Rua é obrigatória.");

                    address.RuleFor(x => x.Number)
                        .GreaterThan(0).WithMessage("Número deve ser maior que 0.");

                    address.RuleFor(x => x.Neighborhood)
                        .NotEmpty().WithMessage("Bairro é obrigatório.");

                    address.RuleFor(x => x.State)
                        .NotEmpty().WithMessage("Estado é obrigatório.");

                    address.RuleFor(x => x.Nickname)
                        .MaximumLength(50).WithMessage("Apelido deve ter no máximo 50 caracteres.");
                });

            RuleFor(x => x.EventJobs)
                .NotNull().WithMessage("A lista de trabalhos do evento não pode ser nula.")
                .ForEach(eventJob =>
                {
                    eventJob.ChildRules(job =>
                    {
                        job.RuleFor(x => x.Title)
                            .NotEmpty().WithMessage("O título do trabalho do evento não pode estar vazio.");

                        job.RuleFor(x => x.Description)
                            .NotEmpty().WithMessage("A descrição do trabalho do evento não pode estar vazia.");

                        job.RuleFor(x => x.MaxVolunteers)
                            .GreaterThan(0).WithMessage("O número máximo de voluntários deve ser maior que 0.");
                    });
                });

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
