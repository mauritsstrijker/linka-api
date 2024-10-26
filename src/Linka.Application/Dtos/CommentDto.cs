using Linka.Domain.Enums;

namespace Linka.Application.Dtos
{
    public class CommentDto
    {
        //Se for org vai vir com Id da org e tipo Org, pra voluntario a mesma coisa
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public UserType Type { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Content { get; set; }
    }
}
