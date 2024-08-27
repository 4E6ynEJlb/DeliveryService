using Domain.Models.Entities.SQLEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesOptions
{
    internal static partial class EntitiesOptions
    {
        internal static void ApplyCardsOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().ToTable("Cards");
            modelBuilder.Entity<Card>().HasKey(c=>c.Number);
            modelBuilder.Entity<Card>().HasIndex(c => c.Number).IsUnique();
            modelBuilder.Entity<Card>().Property(c => c.CVV).IsRequired();
            modelBuilder.Entity<Card>().Property(c => c.Number).IsRequired().HasMaxLength(16);
            modelBuilder.Entity<Card>().Property(c => c.Valid).IsRequired();
            modelBuilder.Entity<Card>().Property(c => c.Holder).IsRequired().HasMaxLength(32);
        }
    }
}
