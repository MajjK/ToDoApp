using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.DB.Model
{
    [Table("Users")]
    public class DbUser
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("addition_date")]
        public DateTime? AdditionDate { get; set; }

        [Column("role")]
        public string Role { get; set; }

        public virtual ICollection<DbTask> Tasks { get; set; }
    }
}
