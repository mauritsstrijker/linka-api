using FluentValidation;
using Linka.Application.Data;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record UpdateEventRequest
      (
            Guid Id,
            string Title,
            string Description,
            DateTime StartDateTime,
            DateTime EndDateTime,
            string? ImageBase64
        ) : IRequest<UpdateEventResponse>;

    public sealed record UpdateEventResponse;

    public class UpdateEventHander
        (
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<UpdateEventRequest, UpdateEventResponse>
    {
        public async Task<UpdateEventResponse> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            var @event = await eventRepository.Get(request.Id, cancellationToken) ?? throw new Exception("Evento nao foi encontrado.");

            var eventImage = request.ImageBase64 is null ? null : Convert.FromBase64String(request.ImageBase64);

            @event.Update(request.Title, request.Description, request.StartDateTime, request.EndDateTime, eventImage);

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

            RuleFor(e => e.StartDateTime)
            .Must(NotBeInThePast)
            .WithMessage("A data de início não pode estar no passado.");

            RuleFor(e => e.EndDateTime)
                .GreaterThan(e => e.StartDateTime)
                .WithMessage("A data de término deve ser posterior à data de início.");

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
