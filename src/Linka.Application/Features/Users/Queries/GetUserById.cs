using Linka.Application.Common;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Users.Queries
{
    public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetUserByIdResponse
    {
        public Guid Id { get; set; }
        public UserType Type { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class GetUserByIdHandler
        (
        IRepository<User> repository
        ) : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var user = await repository.Get(request.Id, cancellationToken) ?? throw new Exception();

            return new GetUserByIdResponse { Id = user.Id, Type = user.Type, Username = user.Username, Email = user.Email };
        }
    }
}
