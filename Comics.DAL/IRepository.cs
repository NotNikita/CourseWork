using Comics.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comics.DAL
{
    public interface IRepository<T> where T : BaseItem
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
