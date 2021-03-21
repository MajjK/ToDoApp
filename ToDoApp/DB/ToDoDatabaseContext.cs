using Microsoft.EntityFrameworkCore;

namespace ToDoApp.DB
{
    public partial class ToDoDatabaseContext : DbContext
    {
        public ToDoDatabaseContext(DbContextOptions<ToDoDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Model.DbTask> Tasks { get; set; }
        public virtual DbSet<Model.DbUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model.DbTask>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(b => b.Tasks);
                entity.Property(c => c.AdditionDate)
                .HasDefaultValueSql("date_trunc('minute'::text, CURRENT_TIMESTAMP)");
                entity.Property(d => d.Finished)
                .HasDefaultValue(false);
            });

            modelBuilder.Entity<Model.DbUser>(entity =>
            {
                entity.HasIndex(a => a.Login)
                .IsUnique();
                entity.Property(b => b.AdditionDate)
                .HasDefaultValueSql("date_trunc('minute'::text, CURRENT_TIMESTAMP)");
                entity.Property(c => c.Role)
                .HasDefaultValue("user");
                entity.Property(d => d.EmailConfirmed)
                .HasDefaultValue(false);
            });
        }
    }
}
