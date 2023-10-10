using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestForWebTechBl.Models;

namespace TestForWebTech.Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    RoleId = 1,
                    Name = "User"
                },
                new Role
                {
                    RoleId = 2,
                    Name = "Admin"
                },
                new Role
                {
                    RoleId = 3,
                    Name = "Support"
                },
                new Role
                {
                    RoleId = 4,
                    Name = "SuperAdmin"
                });;
        }
    }
}
