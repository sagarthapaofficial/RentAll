using System.ComponentModel.DataAnnotations;

namespace RentAll.DTO.Account
{
    public class ConfirmEmailDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [RegularExpression("^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,3}$", ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

    }
}
