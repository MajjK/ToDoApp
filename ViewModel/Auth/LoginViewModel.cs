using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Auth
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
