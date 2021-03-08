using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TaskMetadata
    {
        [Display(Name = "Task")]
        [StringLength(255)]
        public string Objective { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; }

        [Display(Name = "Closing Date")]
        public DateTime? ClosingDate { get; set; }

    }

    public class UserMetadata
    {
        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; }
    }
}
