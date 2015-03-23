using System;
using System.Data.Entity;
using System.Transactions;

namespace Project.Db.Initializers
{
    internal class DropCreateIfModelChangesInitializer<TContext> : BaseInitializer<TContext> where TContext : DbContext
    {
        #region Strategy implementation

        public override void InitializeDatabase(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            bool databaseExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                databaseExists = context.Database.Exists();
            }

            if (databaseExists)
            {
                if (context.Database.CompatibleWithModel(throwIfNoMetadata: true))
                    return;

                context.Database.Delete();
            }

            context.Database.Create();
            Seed(context);
            context.SaveChanges();
        }

        #endregion
    }
}
