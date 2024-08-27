using Domain.Models.Entities.SQLEntities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntitiesOptions;

namespace Infrastructure
{
    public class SQLContext : DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options) { }
        public SQLContext() { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Product> Goods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyUsersOptions();
            modelBuilder.ApplyCardsOptions();
            modelBuilder.ApplyRestaurantsOptions();
            modelBuilder.ApplyGoodsOptions();
            base.OnModelCreating(modelBuilder);
        }
    }
}
