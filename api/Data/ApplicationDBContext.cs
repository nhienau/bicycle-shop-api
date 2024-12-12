using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.ProductDetails)
                .WithMany(e => e.Users)
                .UsingEntity<Cart>();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.ProductDetails)
                .WithMany(e => e.Orders)
                .UsingEntity<OrderDetail>();

            modelBuilder.Entity<ProductDetail>()
                .HasOne(e => e.ProductImage)
                .WithOne(e => e.ProductDetail)
                .HasForeignKey<ProductImage>(e => e.ProductDetailId)
                .IsRequired(false);

            modelBuilder.Entity<Product>()
                .Navigation(p => p.ProductDetails)
                .AutoInclude(false);
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
