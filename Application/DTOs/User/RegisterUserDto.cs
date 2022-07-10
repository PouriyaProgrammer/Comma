using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "ایمیل الزامی است")]
        public string Email { get; set; }

        [Compare("PasswordConfirm", ErrorMessage = "پسورد با تکرار پسورد برابر نیست")]
        [Required(ErrorMessage = "پسورد الزامی است")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا پسورد خود را دوباره وارد نمایید")]
        public string PasswordConfirm { get; set; }
    }
}
