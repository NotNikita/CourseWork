using Comics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comics.DAL
{
    public class CollRepository<T> : ICollRepository<T> where T : Collection
    {
        private ComicsDbContext db;
        private DbSet<T> entities;

        public CollRepository(ComicsDbContext context)
        {
            db = context;
            entities = db.Set<T>();
        }
        void ICollRepository<T>.Delete(T entity)
        {
            entities.Remove(entity);
            db.SaveChanges();
        }

        T ICollRepository<T>.Get(long id)
        {
            return entities.FirstOrDefault(e => e.Id == id);
        }

        IEnumerable<T> ICollRepository<T>.GetAll()
        {
            return entities.AsEnumerable();
        }

        void ICollRepository<T>.Insert(T entity)
        {
            entities.Add(entity);
            db.SaveChanges();

        }

        void ICollRepository<T>.Update(T entity)
        {
            entities.Update(entity);
            db.SaveChanges();
        }

        void ICollRepository<T>.SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
