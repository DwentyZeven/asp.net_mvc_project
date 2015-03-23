using System.Data.Entity;
using Project.Interfaces;

namespace Project.Db.Initializers
{
    internal abstract class BaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public abstract void InitializeDatabase(T context);

        protected virtual void Seed(T context)
        {
            var seeder = context as ISeedDatabase;
            if (seeder != null)
                seeder.Seed();
        }
    }
}
