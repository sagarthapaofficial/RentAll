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
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RentalStatus> RentalStatus { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<Address> Addresss { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }


    }
}
