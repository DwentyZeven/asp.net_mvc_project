using System;
using System.Configuration;
using Project.Interfaces;
using Project.Models;

namespace Project.Db
{
    public partial class ProjectDbContext : ISeedDatabase
    {
        private int _days;

        public void Seed()
        {
            _days = 0;

            var newUser = new User
            {
                VkontakteId = 140153404,
                Firstname = ConfigurationManager.AppSettings["projectName"],
                Lastname = "Admin",
                Gender = 2,
                Email = "",
                Link = "",
                PhotoLink = "",
                Role = 1,
                IsOnline = false,
                IsBanned = false,
                CreatedAt = DateTime.UtcNow
            };
            var user = SeedUser(newUser);
            SeedTickets(user);
        }

        User SeedUser(User newUser)
        {
            User user = Users.Add(newUser);
            SaveChanges();
            return user;
        }

        void SeedTickets(User user)
        {

            Tickets.Add(new Ticket
            {
                User = user,
                Country = "",
                City = "",
                PlaceDescription = "",
                LookDescription = "",
                Gender = 1,
                AgeMin = 20,
                AgeMax = 25,
                Year = 2011,
                Season = 2,
                Month = 7,
                Day = 12,
                AdditionalNote = "",
                Language = 1,
                CreatedAt = DateTime.UtcNow
            });

            SaveChanges();
        }

    }
}
