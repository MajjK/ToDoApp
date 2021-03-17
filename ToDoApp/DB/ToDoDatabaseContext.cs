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
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks);
            });
        }
    }
}
