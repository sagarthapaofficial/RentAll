using RentAll.DAL.Domain_Classes;
using Microsoft.EntityFrameworkCore;

namespace RentAll.DAL
{
    //app extends DbContext
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Address> Addresss { get; set; }


    }
}
