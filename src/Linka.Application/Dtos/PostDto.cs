namespace Linka.Application.Dtos
{
    public class PostDto
    {
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public Guid AssociatedOrganizationId { get; set; }
        public string? ImageBase64 { get; set; }
        public int ShareCount { get; set; }
        public int LikeCount { get; set; }
    }
}
