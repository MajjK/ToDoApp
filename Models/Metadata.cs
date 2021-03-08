using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TaskMetadata
    {
        [Display(Name = "Task")]
        [StringLength(255)]
        public string Objective;

        [StringLength(255)]
        public string Description;

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate;

        [Display(Name = "Closing Date")]
        public DateTime? ClosingDate;
    }

    public class UserMetadata
    {
        [StringLength(50)]
        public string Login;

        [StringLength(50)]
        public string Password;

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate;
    }
}
