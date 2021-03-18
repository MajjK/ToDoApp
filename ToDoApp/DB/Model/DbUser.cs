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
        [Required, StringLength(50)]
        public string Login { get; set; }

        [Column("password")]
        [Required]
        public string Password { get; set; }

        [Column("password_salt")]
        [Required]
        public string PasswordSalt { get; set; }

        [Column("addition_date")]
        [Required]
        public DateTime? AdditionDate { get; set; }

        [Column("role")]
        [Required]
        public string Role { get; set; }

        public virtual ICollection<DbTask> Tasks { get; set; }
    }
}
