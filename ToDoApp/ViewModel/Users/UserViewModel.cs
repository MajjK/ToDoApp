using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoApp.Services;

namespace ToDoApp.ViewModel.Users
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [Required]
        public string Login { get; set; }

        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [Required]
        public string Password { get; set; }

        public string PasswordSalt { get; set; } = HashProfile.GenerateSalt();

        [Display(Name = "Addition Date")]
        public DateTime? AdditionDate { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

        public string Role { get; set; } = "user";

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = true;

        public virtual ICollection<Tasks.TaskViewModel> Tasks { get; set; }
    }
}
