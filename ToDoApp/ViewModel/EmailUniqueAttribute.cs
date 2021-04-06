using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ToDoApp.DB;

namespace ToDoApp.ViewModel
{
    public class EmailUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var DbContext = (ToDoDatabaseContext)validationContext.GetService(typeof(ToDoDatabaseContext));
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var entity = DbContext.Users.SingleOrDefault(e => e.Email == value.ToString());

            if (entity != null && entity.Login != httpContextAccessor.HttpContext.User.Identity.Name)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use.";
        }
    }
}
