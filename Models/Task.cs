using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
//Display name i typ Boolean
namespace ToDoApp.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Task")]
        public string Objective { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; }
        [Display(Name = "Closing Date")]
        public DateTime? ClosingDate { get; set; }
        [Display(Name = "Finished")]
        public Boolean Finished { get; set; }
        
        [Display(Name = "User")]
        public virtual User User { get; set; }
    }
}
