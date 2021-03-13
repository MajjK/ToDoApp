using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Tasks
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }

        public int UserId { get; set; } = 1;

        [Display(Name = "Task")]
        [StringLength(255)]
        public string Objective { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

        [Display(Name = "Closing Date")]
        public DateTime? ClosingDate { get; set; }

        public bool Finished { get; set; }

        public virtual Users.UserViewModel User { get; set; }
    }
}
