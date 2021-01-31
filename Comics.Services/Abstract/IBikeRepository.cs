using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IBikeRepository
    {
        public IEnumerable<Bike> GetAllBikes();
        public IEnumerable<Bike> GetBikesByCollection(int? id);

        public void AddBike(Bike bike);
        public void DeleteBike(Bike bike);
        public void Update(Bike bike);

        public Bike GetBikeById(int? id);
        public Bike GetBikeByName(string? name);

        public IEnumerable<Comment> GetCommentsByBikeId(int id);
    }
}
