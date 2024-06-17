using System.ComponentModel.DataAnnotations;

namespace RentAll.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Firstname must be at least {2}, and maximum {1} characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Lastname must be at least {2}, and maximum {1} characters")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,3}$", ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Password must be at least {2}, and maximum {1} characters")]
        public string Password { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z] ?\\d[A-Za-z]\\d$", ErrorMessage = "invalid Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [RegularExpression("^\\(?\\d{3}\\)?[- ]?\\d{3}[- ]?\\d{4}$", ErrorMessage = "invalid PhoneNo Format")]
        public string PhoneNumber { get; set; }


    }
}
