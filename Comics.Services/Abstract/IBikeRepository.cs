using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IBikeRepository
    {
        public void AddBike(Bike bike);
        public void DeleteBike(Bike bike);
        public void Update(Bike bike);
    }
}
