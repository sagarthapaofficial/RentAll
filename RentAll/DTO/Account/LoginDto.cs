using System.ComponentModel.DataAnnotations;

namespace RentAll.DTO.Account
{
    public class LoginDto
    {
        [Required (ErrorMessage ="Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }
    }
}
