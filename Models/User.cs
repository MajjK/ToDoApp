using System;
using System.Collections.Generic;

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
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? AdditionDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
