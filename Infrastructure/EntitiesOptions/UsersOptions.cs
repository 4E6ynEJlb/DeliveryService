using Domain.Models.Entities.SQLEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesOptions
{
    internal static partial class EntitiesOptions
    {
        internal static void ApplyUsersOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Password).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.TelegramId).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Login).HasColumnName("_Login").HasMaxLength(16).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("_Password").HasMaxLength(16).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Name).HasColumnName("_Name").HasMaxLength(16).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Bonuses).IsRequired().HasColumnType("decimal(18,2)");
            modelBuilder.Entity<User>().Property(u => u.IsAdmin).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.TelegramId).HasMaxLength(32);
            modelBuilder.Entity<User>().HasMany(u => u.Cards).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        }
    }
}
