using Linka.Application.Dtos;

namespace Linka.Application.Common
{
    public record BaseRegisterRequest
        (
          string Username,
          string Password,
          string Email,
          CreateAddressDto Address,
          byte[]? ProfilePictureBytes
        );
}
