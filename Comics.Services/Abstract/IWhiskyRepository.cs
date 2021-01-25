using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IWhiskyRepository
    {
        public IEnumerable<Whisky> GetAllWhiskies();
        public IEnumerable<Whisky> GetWhiskiesByCollection(int? id);

        public void AddWhisky(Whisky whisky);
        public void DeleteWhisky(Whisky whisky);
        public void Update(Whisky whisky);

        public Whisky GetWhiskyById(int? id);
        public Whisky GetWhiskyByName(string? name);
    }
}
