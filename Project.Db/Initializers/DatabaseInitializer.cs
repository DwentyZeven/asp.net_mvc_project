using System;
using System.Data.Entity;
using Project.Interfaces;

namespace Project.Db.Initializers
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseInitializer(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;

            #if DEBUG

            Database.SetInitializer(new DropCreateAlwaysInitializer<ProjectDbContext>());

            #endif
        }

        protected ProjectDbContext Context
        {
            get { return (ProjectDbContext) _unitOfWork; }
        }

        public void Initialize()
        {
            #if DEBUG

            Context.Database.ExecuteSqlCommand("CREATE USER [IIS APPPOOL\\fromsong.ru] FOR LOGIN [IIS APPPOOL\\fromsong.ru]");
            Context.Database.ExecuteSqlCommand("EXEC sp_addrolemember 'db_datareader', 'IIS APPPOOL\\fromsong.ru'");
            Context.Database.ExecuteSqlCommand("EXEC sp_addrolemember 'db_datawriter', 'IIS APPPOOL\\fromsong.ru'");

            Context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [Ticket_User]");
            Context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [Ticket_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE SET NULL");
            Context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [Ticket_User]");

            #endif
        }
    }
}
