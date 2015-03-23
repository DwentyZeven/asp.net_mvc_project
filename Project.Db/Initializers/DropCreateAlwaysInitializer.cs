using System;
using System.Data.Entity;

namespace Project.Db.Initializers
{
    internal class DropCreateAlwaysInitializer<TContext> : BaseInitializer<TContext> where TContext : DbContext
    {
        #region Strategy implementation

        public override void InitializeDatabase(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Database.Exists())
            {
                context.Database.Delete();
            }

            context.Database.Create();
            Seed(context);
            context.SaveChanges();
        }

        #endregion
    }
}
