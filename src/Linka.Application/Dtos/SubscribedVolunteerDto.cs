namespace Linka.Application.Dtos
{
    public sealed record SubscribedVolunteerDto(Guid Id, string CPF, string FullName, string? ProfilePictureBase64, DateTime? CheckIn, DateTime? CheckOut);
}
