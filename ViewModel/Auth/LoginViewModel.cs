using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Auth
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
