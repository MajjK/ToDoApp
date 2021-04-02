using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel.Auth
{
    public class EmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
