using Linka.Application.Dtos;

namespace Linka.Application.Common
{
    public record BaseRegisterRequest
        (
          string Username,
          string Password,
          string Email,
          AddressDto Address,
          string? ProfilePictureBase64
        );
}
