using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Project.Db;
using Project.Interfaces;

namespace Project.Services
{
    public class BaseRepository
    {
        private int _pageIndex;

        private int _pageSize;

        protected IUnitOfWork UnitOfWork { get; set; }

        protected ProjectDbContext Context
        {
            get { return (ProjectDbContext) UnitOfWork; }
        }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
            UnitOfWork = unitOfWork;
        }

        protected virtual DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        protected virtual void SetEntityState(object entity, EntityState entityState)
        {
            Context.Entry(entity).State = entityState;
        }

        protected IOrderedQueryable<TEntity> TakePageItems<TEntity>(IOrderedQueryable<TEntity> entity) where TEntity : class
        {
            return (IOrderedQueryable<TEntity>) entity.Skip(_pageIndex * _pageSize).Take(_pageSize);
        }

        public void SetPaginationParams(int pageIndex, int pageSize)
        {
            _pageIndex = pageIndex;
            _pageSize = pageSize;
        }
    }
}
