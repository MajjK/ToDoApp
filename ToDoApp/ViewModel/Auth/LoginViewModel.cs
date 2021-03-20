using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Auth
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string Password { get; set; }
    }
}
