using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using System;
using System.Collections.Generic;
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
    }
}
