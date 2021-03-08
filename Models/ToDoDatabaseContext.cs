using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ToDoApp.Models
{
    public partial class ToDoDatabaseContext : DbContext
    {
        public ToDoDatabaseContext(DbContextOptions<ToDoDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_Poland.1250");

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskId)
                    .HasColumnName("task_id")
                    .HasDefaultValueSql("nextval('task_task_id_seq'::regclass)");

                entity.Property(e => e.AdditionDate)
                    .HasColumnName("addition_date")
                    .HasDefaultValueSql("date_trunc('minute'::text, CURRENT_TIMESTAMP)");

                entity.Property(e => e.ClosingDate).HasColumnName("closing_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Finished).HasColumnName("finished");

                entity.Property(e => e.Objective)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("objective");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasDefaultValueSql("nextval('task_operator_id_seq'::regclass)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("task_operator_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login, "Users_login_key")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasDefaultValueSql("nextval('operator_operator_id_seq'::regclass)");

                entity.Property(e => e.AdditionDate)
                    .HasColumnName("addition_date")
                    .HasDefaultValueSql("date_trunc('minute'::text, CURRENT_TIMESTAMP)");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
