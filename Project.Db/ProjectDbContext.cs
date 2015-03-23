using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using Project.Interfaces;
using Project.Models;

namespace Project.Db
{
    public partial class ProjectDbContext : DbContext, IUnitOfWork
    {
        public ProjectDbContext() : base(ConfigurationManager.AppSettings["projectName"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetupTicketEntity(modelBuilder);
            SetupUserEntity(modelBuilder);
        }

        private static void SetupTicketEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasKey(t => t.TicketId);
            modelBuilder.Entity<Ticket>().Property(t => t.TicketId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Ticket>().Property(t => t.Country).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.Country).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.City).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.City).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.PlaceDescription).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.PlaceDescription).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.LookDescription).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.LookDescription).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.Gender).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.AgeMin).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.AgeMax).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.Year).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Season).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Month).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.Day).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.AdditionalNote).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.AdditionalNote).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.Firstname).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.Firstname).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.Lastname).IsOptional();
            modelBuilder.Entity<Ticket>().Property(t => t.Lastname).HasMaxLength(255);
            modelBuilder.Entity<Ticket>().Property(t => t.Language).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.CreatedAt).IsRequired();
            modelBuilder.Entity<Ticket>().HasOptional(t => t.User)
                                         .WithMany(u => u.Tickets)
                                         .Map(m => m.MapKey("UserId"))
                                         .WillCascadeOnDelete(false);
        }

        private static void SetupUserEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().Property(u => u.FacebookId).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.VkontakteId).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Firstname).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Firstname).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Lastname).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Lastname).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Gender).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Username).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.Username).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Email).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Link).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Link).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.PhotoLink).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.PhotoLink).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Role).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsOnline).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsWarned).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsBanned).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).IsRequired();
        }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<User> Users { get; set; }

        void IUnitOfWork.SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
