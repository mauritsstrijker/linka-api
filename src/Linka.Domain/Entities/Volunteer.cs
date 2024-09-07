using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Volunteer : BaseEntity
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName
        {
            get { return Name + " " + Surname; }
        }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public User User { get; set; }
        public int Points { get; set; }
        public int AllTimePoints { get; set; }

        public static Volunteer Create
            (
            string cpf,
            string name,
            string surname,
            Address address,
            DateTime dateOfBirth,
            User user
            )
        {
            return new Volunteer
            {
                Id = Guid.NewGuid(),
                CPF = cpf.Trim(),
                Name = name.Trim(),
                Surname = surname.Trim(),
                Address = address,
                DateOfBirth = dateOfBirth,
                User = user,
                Points = 0,
                AllTimePoints = 0
            };
        }
    }
}
