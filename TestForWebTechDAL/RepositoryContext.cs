using Microsoft.EntityFrameworkCore;
using TestForWebTech.Entities.Configuration;
using TestForWebTechBl.Models;
using TestForWebTechDAL.Entities.Configuration;

namespace TestForWebTechDAL
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.UserId);
            modelBuilder.Entity<User>().Property(x => x.UserId);
            modelBuilder.Entity<User>().Property(x => x.Name);
            modelBuilder.Entity<User>().HasIndex(x => x.Name);
            modelBuilder.Entity<User>().Property(x => x.Email);
            modelBuilder.Entity<User>().HasIndex(x => x.Email);
            modelBuilder.Entity<User>().Property(x => x.Age);
            modelBuilder.Entity<User>().HasIndex(x => x.Age);

            modelBuilder.Entity<Role>().HasKey(x => x.RoleId);
            modelBuilder.Entity<Role>().Property(x => x.RoleId);
            modelBuilder.Entity<Role>().Property(x => x.Name);
            modelBuilder.Entity<Role>().HasIndex(x => x.Name);

            modelBuilder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<UserRole>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<UserRole>().Property(x => x.RoleId).IsRequired();
            modelBuilder.Entity<UserRole>().HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserRole>().HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
