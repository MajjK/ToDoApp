using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ToDoApp.Models
{
    public partial class User
    {
        public User()
        {
            Tasks = new HashSet<Task>();
        }

        public int UserId { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        public string Password { get; set; }
        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; }

        [Display(Name = "Tasks")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
