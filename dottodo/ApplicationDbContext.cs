using dottodo.Board;
using dottodo.Card;
using dottodo.Common;
using dottodo.Task;
using dottodo.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dottodo
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<BoardEntity> Boards { get; set; }
        
        public DbSet<CardEntity> Cards { get; set; }

        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntity>()
                .HasMany(u => u.Boards)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .IsRequired();

            builder.Entity<BoardEntity>()
                .HasMany(b => b.Cards)
                .WithOne(c => c.Board)
                .HasForeignKey(c => c.BoardId)
                .IsRequired();

            builder.Entity<CardEntity>()
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Card)
                .HasForeignKey(t => t.CardId)
                .IsRequired();
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public void AddTimestamps()
        {
            var models = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var model in models)
            {
                var now = DateTime.UtcNow; 

                if (model.State == EntityState.Added)
                {
                    ((BaseEntity)model.Entity).CreatedAt = now;
                }

                ((BaseEntity)model.Entity).UpdatedAt = now;
            }
        }
    }
}
