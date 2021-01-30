using Comics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comics.DAL
{
    public class Repository<T> : IRepository<T> where T : BaseItem
    {
        private ComicsDbContext db;
        private DbSet<T> entities;

        public Repository(ComicsDbContext context)
        {
            db = context;
            entities = db.Set<T>();
        }
        void IRepository<T>.Delete(T entity)
        {
            entities.Remove(entity);
            db.SaveChanges();
        }

        T IRepository<T>.Get(long id)
        {
            return entities.FirstOrDefault(e => e.Id == id);
        }

        IEnumerable<T> IRepository<T>.GetAll()
        {
            return entities.AsEnumerable();
        }

        void IRepository<T>.Insert(T entity)
        {
            entities.Add(entity);
            db.SaveChanges();
        }

        void IRepository<T>.SaveChanges()
        {
            db.SaveChanges();
        }

        void IRepository<T>.Update(T entity)
        {
            entities.Update(entity);
            db.SaveChanges();
        }
    }
}
