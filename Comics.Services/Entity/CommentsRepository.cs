using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comics.Domain;
using Comics.DAL;
using Comics.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Comics.Services.Entity
{
    public class CommentsRepository : ICommentsRepository
    {
        private ComicsDbContext db;
        private DbSet<Comment> entities;

        public CommentsRepository(ComicsDbContext _db)
        {
            db = _db;
            entities = db.Set<Comment>();
        }

        void ICommentsRepository.AddComm(Comment comm)
        {
            entities.Add(comm);
            db.SaveChanges();
        }

        void ICommentsRepository.RemoveComm(Comment comm)
        {
            entities.Remove(comm);
            db.SaveChanges();
        }
    }
}
