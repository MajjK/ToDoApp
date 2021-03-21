using System;
using System.ComponentModel.DataAnnotations;
using ToDoApp.Services;


namespace ToDoApp.ViewModel.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public string PasswordSalt { get; set; } = HashProfile.GenerateSalt();

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

    }
}
