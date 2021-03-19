using System;
using System.ComponentModel.DataAnnotations;
using ToDoApp.Services;


namespace ToDoApp.ViewModel.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public string PasswordSalt { get; set; } = HashProfile.GenerateSalt();

    }
}
