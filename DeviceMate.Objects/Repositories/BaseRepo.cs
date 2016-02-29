using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DeviceMate.Models.Entities;
using Microsoft.Practices.Unity;
using System.Linq.Expressions;

namespace DeviceMate.Objects.Repositories
{
    public abstract class BaseRepo<T> where T : class
    {
        #region Private Variables

        private DbSet<T> objectSet;

        private DbSet<T> ObjectSet
        {
            get
            {
                if (this.objectSet == null)
                {
                    this.objectSet = this.Context.Set<T>();
                }

                return this.objectSet;
            }
        }

        #endregion

        #region Protected Properties
        DeviceContext _context;

        public DeviceContext Context
        {
            get { return _context; }
            set
            {
                _context = value;
                this.objectSet = this.Context.Set<T>();
            }
        }

        #endregion

        #region Constructor

        [InjectionConstructor]
        public BaseRepo(DeviceContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Public Methods

        public virtual void Add(T entity)
        {
            this.ObjectSet.Add(entity);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                this.ObjectSet.Add(entity);
            }
        }

        public virtual void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public virtual void Attach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                this.ObjectSet.Attach(entity);
            }
        }

        public virtual int Create(T entity)
        {
            this.ObjectSet.Add(entity);

            return this.SaveChanges();
        }

        public virtual void Create(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                this.ObjectSet.Add(entity);
            }

            this.SaveChanges();
        }

        public virtual int Delete(T entity)
        {
            this.ObjectSet.Remove(entity);

            return this.SaveChanges();
        }

        public virtual int Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                this.ObjectSet.Remove(entity);
            }

            return this.SaveChanges();
        }

        public virtual void DeleteWithoutSave(T entity)
        {
            this.ObjectSet.Remove(entity);
        }

        public virtual void DeleteWithoutSave(int id)
        {
            T entity = this.ObjectSet.Find(id);
            this.ObjectSet.Remove(entity);
        }

        public virtual void DeleteWithoutSave(IEnumerable<T> entities)
        {
            this.ObjectSet.RemoveRange(entities);
            //foreach (var entity in entities)
            //{
            //    this.ObjectSet.Remove(entity);
            //}
        }

        public virtual int SaveChanges()
        {
            Context.ChangeTracker.DetectChanges();
            return this.Context.SaveChanges();
        }

        public virtual int GetAllCount()
        {
            return this.ObjectSet.Count();
        }

        public virtual IQueryable<T> GetNoTracking(Expression<Func<T, bool>> filter)
        {
            return this.ObjectSet.AsNoTracking().Where(filter);
        }

        public virtual IQueryable<T> GetNoTracking()
        {
            return this.ObjectSet.AsNoTracking();
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return this.ObjectSet.Where(filter);
        }

        public virtual T GetById(object id)
        {
            return this.ObjectSet.Find(id);
        }

        /// <summary>
        /// Update entity changes and save to database - use only for instances not from current UnitOfWork.
        /// Be careful for foreign keys.
        /// </summary>
        /// <param name="entity">Specified the entity to save. </param>
        public virtual void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            this.ObjectSet.Add(entity);
            entry.State = EntityState.Modified;
        }

        #endregion
    }
}
