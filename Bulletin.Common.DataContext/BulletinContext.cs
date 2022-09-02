using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BulletinApp.Shared
{
    public partial class BulletinContext : DbContext
    {
        public BulletinContext()
        {
        }

        public BulletinContext(DbContextOptions<BulletinContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Promo> Promos { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bulletin;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promo>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Promos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Promos_User");

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Promos)
                    .UsingEntity<Dictionary<string, object>>(
                        "PromosCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_Categories"),
                        r => r.HasOne<Promo>().WithMany().HasForeignKey("PromoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_Promos"),
                        //pc => pc.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_Categories"),
                        //pc => pc.HasOne<Promo>().WithMany().HasForeignKey("PromoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_Promos"),
                        j =>
                        {
                            j.HasKey("PromoId", "CategoryId");

                            j.ToTable("PromosCategories");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
