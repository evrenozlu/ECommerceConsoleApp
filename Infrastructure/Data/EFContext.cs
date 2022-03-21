using Domain.Campaigns;
using Domain.Orders;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
    }
}