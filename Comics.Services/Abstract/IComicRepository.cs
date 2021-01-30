using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IComicRepository
    {
        public IEnumerable<Comic> GetAllComics();
        public IEnumerable<Comic> GetComicsByCollection(int? id);

        public void AddComic(Comic comic);
        public void DeleteComic(Comic comic);
        public void Update(Comic comic);

        public Comic GetComicById(int? id);
        public Comic GetComicByName(string? name);

        public IEnumerable<Comment> GetCommentsByComicId(int id);
    }
}
