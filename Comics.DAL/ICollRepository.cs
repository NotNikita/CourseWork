using Comics.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comics.DAL
{

    public interface ICollRepository<T> where T : Collection
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
