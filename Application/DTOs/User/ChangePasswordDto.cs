using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class ChangePasswordDto
    {
        public string BefourePass { get; set; }

        [Compare("NewPassConfirm")]
        public string NewPass { get; set; }

        public string NewPassConfirm { get; set; }
    }
}
