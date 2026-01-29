using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Firstname).IsRequired();
            builder.Property(x => x.Lastname).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.LastLogin).IsRequired(false);
            builder.Property(x => x.DeletedAt).IsRequired(false); 
            builder.Property(x => x.FailedLoginCount).IsRequired();

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(255);
            });

            builder.OwnsOne(x => x.PasswordHash, ph =>
            {
                ph.Property(p => p.Value).HasColumnName("PasswordHash").IsRequired();
            });

            builder.OwnsOne(x => x.Role, ph =>
            {
                ph.Property(p => p.Value).HasColumnName("Role").IsRequired();
            });

            builder.Property(x => x.Status).IsRequired();
        }
    }
}

