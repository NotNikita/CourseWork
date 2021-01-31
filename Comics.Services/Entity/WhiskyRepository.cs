using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comics.Services.Entity
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private ComicsDbContext db;
        private IRepository<Whisky> whiskiesRep;

        public WhiskyRepository(ComicsDbContext db, IRepository<Whisky> whiskiesRep)
        {
            this.db = db;
            this.whiskiesRep = whiskiesRep;
        }

        public void AddWhisky(Whisky whisky)
        {
            whiskiesRep.Insert(whisky);
        }

        public void DeleteWhisky(Whisky whisky)
        {
            whiskiesRep.Delete(whisky);
        }

        public void Update(Whisky whisky)
        {
            whiskiesRep.Update(whisky);
        }

        public IEnumerable<Whisky> GetAllWhiskies()
        {
            return whiskiesRep.GetAll();
        }

        public IEnumerable<Whisky> GetWhiskiesByCollection(int? id)
        {
            return db.Whiskies.Where(com => com.CollectionId == id).ToList();
        }

        public Whisky GetWhiskyById(int? id)
        {
            return db.Whiskies.Include(c => c.Likes)
                .Include(c => c.Comments)
                .FirstOrDefault(c => c.Id == id);
        }

        public Whisky GetWhiskyByName(string name)
        {
            return db.Whiskies.Where(com => com.Name == name).FirstOrDefault();
        }

        public IEnumerable<Comment> GetCommentsByWhiskyId(int id)
        {
            return db.Comments.Where(comm => comm.ItemId == id).ToList();
        }
    }
}
