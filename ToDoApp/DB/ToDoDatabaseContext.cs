using Microsoft.EntityFrameworkCore;
using System;

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
                entity.HasIndex(a => a.Email)
                .IsUnique();
                entity.Property(b => b.AdditionDate)
                .HasDefaultValueSql("date_trunc('minute'::text, CURRENT_TIMESTAMP)");
                entity.Property(c => c.Role)
                .HasDefaultValue("user");
                entity.Property(d => d.EmailConfirmed)
                .HasDefaultValue(false);
            });

            modelBuilder.Entity<Model.DbUser>().HasData
                (
                new Model.DbUser{UserId = 1, Login = "postgres", Password = "HcaVRCH9ST0+WePw59Qv5ghRuB1M14a/M73xT+BPxHYVtOSkZ1MG38NnYTECfBCM0duVC4+hnNEVTXVWDPxCeg==",
                    PasswordSalt = "��K��3���I<e	0n¼��gy�A�n*(b�Q�,1�A`���Q@5l��B�1����o", AdditionDate = DateTime.Parse("2021-03-24 00:00:00"), Role = "admin",
                    Email = "test@gmail.com", EmailConfirmed = true },
                new Model.DbUser{UserId = 2, Login = "postgres2", Password = "Q9A/L2XTa9kOjCU2QnQ1Dt+YLGv0C7iqjsdoW04J+RkVuwbwr+Qy8ZweU+JTamVBy+WDxs1CBCovlqN+0rXDtw==", 
                    PasswordSalt = "3oY�-S7���Ѽ��'�A�!NɅ����Oi��8�P^}g�	�=´��H����:X�Y", AdditionDate = DateTime.Parse("2021-03-24 00:00:00"), Role = "user",
                    Email = "test2@gmail.com", EmailConfirmed = true }
                );

            modelBuilder.Entity<Model.DbTask>().HasData
                (
                new Model.DbTask{TaskId = 1, UserId = 1, Objective = "Example Task #1 User #1", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 16:00:00"), 
                    ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new Model.DbTask{TaskId = 2, UserId = 1, Objective = "Example Task #2 User #1", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"), 
                    ClosingDate = DateTime.Parse("2021-03-26 00:00:00"), Finished = false},
                new Model.DbTask{TaskId = 3, UserId = 1, Objective = "Example Task #3 User #1", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                    ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new Model.DbTask{TaskId = 4, UserId = 1, Objective = "Example Task #4 User #1", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                    ClosingDate = DateTime.Parse("2021-03-30 00:00:00"), Finished = false},
                new Model.DbTask{TaskId = 5, UserId = 2, Objective = "Example Task #1 User #2", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                    ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new Model.DbTask{TaskId = 6, UserId = 2, Objective = "Example Task #2 User #2", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                    ClosingDate = DateTime.Parse("2021-03-30 00:00:00"), Finished = false}
                );
        }
    }
}
