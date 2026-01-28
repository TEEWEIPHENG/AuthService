using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<User>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.GetType()
                        .GetProperty("UpdatedAt")?
                        .SetValue(entry.Entity, DateTime.UtcNow);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
