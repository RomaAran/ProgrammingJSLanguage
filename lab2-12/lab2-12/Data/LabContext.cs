using Microsoft.EntityFrameworkCore;
using lab2_12.Models;

namespace lab2_12.Data
{
    public class LabContext : DbContext
    {
        public LabContext(DbContextOptions<LabContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Login)
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(50);
            });
        }
    }
}