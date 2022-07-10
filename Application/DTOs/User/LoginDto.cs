using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string Username { get; set; }

        [Required(ErrorMessage = "پسورد الزامی است")]
        public string Password { get; set; }
    }
}
