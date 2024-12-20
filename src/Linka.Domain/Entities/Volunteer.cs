﻿using Linka.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
        public byte[]? ProfilePictureBytes { get; set; }
        public string? ProfilePictureExtension { get; set; }
        public List<EventJob> Jobs { get; set; }
        [InverseProperty("Requester")]
        public List<ConnectionRequest> SentRequests { get; set; }

        [InverseProperty("Target")]
        public List<ConnectionRequest> ReceivedRequests { get; set; }
        [InverseProperty("Volunteer1")]
        public List<Connection> ConnectionsAsVolunteer1 { get; set; }

        [InverseProperty("Volunteer2")]
        public List<Connection> ConnectionsAsVolunteer2 { get; set; }
        public List<ProductReservation> ProductReservations { get; set; }
        public static Volunteer Create
            (
            string cpf,
            string name,
            string surname,
            Address address,
            DateTime dateOfBirth,
            User user,
            byte[]? profilePictureBytes = null,
            string? profilePictureExtension = null
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
                AllTimePoints = 0,
                ProfilePictureBytes = profilePictureBytes,
                ProfilePictureExtension = profilePictureExtension
            };
        }
    }
}
