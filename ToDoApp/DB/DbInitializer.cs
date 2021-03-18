using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.DB.Model;

namespace ToDoApp.DB
{
    public class DbInitializer
    {
        public static void Initialize(ToDoDatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   
            }

            var users = new DbUser[]
            {
            new DbUser{UserId = 1, Login = "postgres", Password = "HcaVRCH9ST0+WePw59Qv5ghRuB1M14a/M73xT+BPxHYVtOSkZ1MG38NnYTECfBCM0duVC4+hnNEVTXVWDPxCeg==",
                PasswordSalt = "��K��3���I<e	0n¼��gy�A�n*(b�Q�,1�A`���Q@5l��B�1����o", AdditionDate = DateTime.Parse("2021-03-24 00:00:00"), Role = "admin"},
            new DbUser{UserId = 2, Login = "postgres2", Password = "Q9A/L2XTa9kOjCU2QnQ1Dt+YLGv0C7iqjsdoW04J+RkVuwbwr+Qy8ZweU+JTamVBy+WDxs1CBCovlqN+0rXDtw==", 
                PasswordSalt = "3oY�-S7���Ѽ��'�A�!NɅ����Oi��8�P^}g�	�=´��H����:X�Y", AdditionDate = DateTime.Parse("2021-03-24 00:00:00"), Role = "user"},
            };
            foreach (DbUser user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            var tasks = new DbTask[]
            {
                new DbTask{TaskId = 1, UserId = 1, Objective = "Example Task #1 User #1", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 16:00:00"), 
                ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new DbTask{TaskId = 2, UserId = 1, Objective = "Example Task #2 User #1", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"), 
                ClosingDate = DateTime.Parse("2021-03-26 00:00:00"), Finished = false},
                new DbTask{TaskId = 3, UserId = 1, Objective = "Example Task #3 User #1", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new DbTask{TaskId = 4, UserId = 1, Objective = "Example Task #4 User #1", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                ClosingDate = DateTime.Parse("2021-03-30 00:00:00"), Finished = false},
                new DbTask{TaskId = 5, UserId = 2, Objective = "Example Task #1 User #2", Description = "Example Description", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                ClosingDate = DateTime.Parse("2021-03-24 00:00:00"), Finished = true},
                new DbTask{TaskId = 6, UserId = 2, Objective = "Example Task #2 User #2", AdditionDate = DateTime.Parse("2021-03-18 00:00:00"),
                ClosingDate = DateTime.Parse("2021-03-30 00:00:00"), Finished = false},
            };
            foreach (DbTask task in tasks)
            {
                context.Tasks.Add(task);
            }
            context.SaveChanges();
        }
    }
}
