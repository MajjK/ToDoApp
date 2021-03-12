using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoApp.Models;

namespace ToDoApp.ViewModel.Users
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime? AdditionDate { get; set; }

        public virtual ICollection<Tasks.TaskViewModel> Tasks { get; set; }
    }
}
