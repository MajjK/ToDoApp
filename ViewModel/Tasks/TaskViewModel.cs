using System;

namespace ToDoApp.ViewModel.Tasks
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string Objective { get; set; }

        public string Description { get; set; }

        public DateTime? AdditionDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public bool Finished { get; set; }

        public virtual Users.UserViewModel User { get; set; }
    }
}
