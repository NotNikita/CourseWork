using System.Linq;
using Comics.Domain;
using Comics.DAL;
using Comics.Services.Abstract;
using Microsoft.EntityFrameworkCore;


namespace Comics.Services.Entity
{
    public class UserRepository : IUserRepository
    {
        private ComicsDbContext db;

        public UserRepository(ComicsDbContext _context)
        {
            db = _context;
        }
        public User GetUserDb(string id)
        {
            return db.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserInfo(string id)
        {
            return db.Users.Include(u => u.Collections)
                .Include(u => u.WishList)
                .FirstOrDefault(u => u.Id == id);
        }

        void IUserRepository.AddUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        void IUserRepository.DeleteUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
