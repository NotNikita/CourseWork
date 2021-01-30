using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comics.Services.Entity
{
    public class ComicRepository : IComicRepository
    {
        private ComicsDbContext db;
        private IRepository<Comic> comicsRep;

        public ComicRepository(ComicsDbContext db, IRepository<Comic> comicsRep)
        {
            this.db = db;
            this.comicsRep = comicsRep;
        }

        public void AddComic(Comic comic)
        {
            comicsRep.Insert(comic);
        }

        public void DeleteComic(Comic comic)
        {
            comicsRep.Delete(comic);
        }

        public void Update(Comic comic)
        {
            comicsRep.Update(comic);
        }

        public IEnumerable<Comic> GetAllComics()
        {
            return comicsRep.GetAll();
        }

        public IEnumerable<Comic> GetComicsByCollection(int? id)
        {
            return db.Comics.Where(com => com.CollectionId == id).ToList();
        }

        public Comic GetComicById(int? id)
        {
            return db.Comics.Where(com => com.Id == id).FirstOrDefault();
        }

        public Comic GetComicByName(string name)
        {
            return db.Comics.Where(com => com.Name == name).FirstOrDefault();
        }

        public IEnumerable<Comment> GetCommentsByComicId(int id)
        {
            return db.Comments.Where(comm => comm.ItemId == id).ToList();
        }
    }
}
