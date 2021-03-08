using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace ToDoApp.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Task")]
        [StringLength(255)]
        public string Objective { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; }

        [Display(Name = "Closing Date")]
        public DateTime? ClosingDate { get; set; }

        public Boolean Finished { get; set; }
        
        public virtual User User { get; set; }
    }
}
