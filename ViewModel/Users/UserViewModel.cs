using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Users
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

        public string Role { get; set; } = "user";

        public virtual ICollection<Tasks.TaskViewModel> Tasks { get; set; }
    }
}
