using System;
using System.Collections.Generic;

#nullable disable

namespace ToDoApp.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public DateTime? AdditionDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool Finished { get; set; }

        public virtual User User { get; set; }
    }
}
