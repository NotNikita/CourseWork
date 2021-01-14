using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using System;
using System.Collections.Generic;
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
    }
}
