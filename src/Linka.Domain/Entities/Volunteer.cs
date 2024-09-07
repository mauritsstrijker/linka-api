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
    }
}
