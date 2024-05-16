using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using RentAll.DAL.Domain_Classes;
namespace RentAll.DAL.DAO
{
    public class UserDAO
    {
        private AppDbContext _db;
        public UserDAO(AppDbContext context)
        {
            _db = context;
        }

        //to register user.
        public async Task<User>Register(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        //get user by email.
        public async Task<User> GetByEmail(string email)
        {
            //get the first or default result after finding matching email
            User User = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
            return User;
        }


    }
}
