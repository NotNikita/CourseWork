using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IComicRepository
    {
        public void AddComic(Comic comic);
        public void DeleteComic(Comic comic);
        public void Update(Comic comic);
    }
}
