using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestForWebTechBl.Models;

namespace TestForWebTech.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.Email).IsRequired();

            builder.HasData
                (
                    new User
                    {
                        UserId = 1,
                        Name = "Barsik",
                        Age = 25,
                        Email = "tester@gmail.com"
                    },

                    new User
                    {
                        UserId = 2,
                        Name = "Dima",
                        Age = 25,
                        Email = "tes@gmail.com"
                    },

                     new User
                     {
                         UserId = 3,
                         Name = "Barserk",
                         Age = 21,
                         Email = "teska@gmail.com"
                     }
                );
    }

    }
} 

