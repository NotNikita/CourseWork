using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comics.Services.Entity
{
    public class BikeRepository : IBikeRepository
    {
        private ComicsDbContext db;
        private IRepository<Bike> bikesRep;

        public BikeRepository(ComicsDbContext db, IRepository<Bike> bikesRep)
        {
            this.db = db;
            this.bikesRep = bikesRep;
        }

        public void AddBike(Bike bike)
        {
            bikesRep.Insert(bike);
        }

        public void DeleteBike(Bike bike)
        {
            bikesRep.Delete(bike);
        }

        public void Update(Bike bike)
        {
            bikesRep.Update(bike);
        }

        public IEnumerable<Bike> GetAllBikes()
        {
            return bikesRep.GetAll();
        }

        public Bike GetBikeById(int? id)
        {
            return db.Bikes.Where(com => com.Id == id).FirstOrDefault();
        }

        public Bike GetBikeByName(string name)
        {
            return db.Bikes.Where(com => com.Name == name).FirstOrDefault();
        }

        public IEnumerable<Bike> GetBikesByCollection(int? id)
        {
            return db.Bikes.Where(com => com.CollectionId == id).ToList();
        }

    }
}
